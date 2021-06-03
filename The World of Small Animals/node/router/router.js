'use strict'

const cors = require('cors');

const dateFormat = require('dateformat');


const config = require('../config.js')
const encrypter = require('../crypto/encypter')

const bencrypter = require('../crypto/bencrypter')
const mongoose = require("mongoose");
const express = require('express');

const helmet = require('helmet')

const bodyParser = require('body-parser');
const  validatorEmail = require("email-validator");

const fs = require('fs');

const pathPublicFolber = "/public/"



//#region Schemas

const schemaUser = require('../models/user');

const schemaCharacter = require('../models/character');

const schemaServer = require('../models/server');

const schemaTokenUser = require('../models/tokenUser');

const TokenUser = mongoose.model("TokenUser", schemaTokenUser);

const Server = mongoose.model("Server", schemaServer);

const User = mongoose.model("User", schemaUser);

const Character = mongoose.model("Character", schemaCharacter);

//#endregion

//#region  Prototypes

Array.prototype.contains = function(value) {
  return   this.includes(value)
}

Object.prototype.isNaN = function() {
    return    typeof this === Number
  }
    
//#endregion

const router = express.Router();
router.use(cors());
router.use(helmet())

router.use(bodyParser.json());
router.use(bodyParser.urlencoded({ extended: true }));

start()

async function start() {
    try {
        await mongoose.connect(config.url, {
            useNewUrlParser: true,
            useFindAndModify: false
        })
    } catch (e) {
        return console.error(err);
    }
}

function clearDataUser(user) {
    user.password = undefined;
    user.email = undefined;
    user.__v = undefined;
}

function clearDataCharacter(character) {
    character.userId = undefined
}

function check_token_date(token) {
    const dateNow = new Date()


    const dateToken = token.time
     const conditions = dateToken.getDay() != dateNow.getDay() + 1 || dateToken.getMonth() != dateToken.getMonth() ||  dateToken.getFullYear() != dateToken.getFullYear()

     return conditions
}

async function playerContainsOnServer(name) {

    var result = false
    await   Server.find({}, function(err, docs){
        if(err) {
            return console.error(err);
        }
        
        for (let index = 0; index < docs.length; index++) {
            const server = docs[index];
            
            if (server.players.contains(name)) {
                result = true
                break
            }
        }
    })

    return result
}

function convertToInt(value) {
    value = value.isNaN(value) ? Number.parseInt(value) : value
    return value
}




// define the home page route
router.get('/', function(req, res) {
  res.send('home api');
});



router.get('/users', async function (req, res) {
 await   User.find({}, function(err, docs){
         
        if(err) {
            res.sendStatus(500)
            return console.log(err);
        } 

        for (let index = 0; index < docs.length; index++) {
            clearDataUser(docs[index])
        }
         

        res.json(docs)
    });

});


   
   



 router.delete('/users/auth/:id', async function (req, res) {
    if (!req.body.id) {
        res.sendStatus(400)
        return
    }
 
    await TokenUser.deleteOne({idUser: req.params.id},  async function(err, result) {
        
        if(err) {
            res.sendStatus(500)
            return console.log(err);
        } 
 
       if (result.deletedCount < 1) {
            res.sendStatus(404)
       }
 
       else {
        
        res.sendStatus(204)
       }
       
    })
 });



 router.post('/users/auth/', async function (req, res) {
    if(!req.body) return res.sendStatus(400);

    console.log(`new auth operation:\nData:\n${JSON.stringify(req.body)}`)

    const validateData = [
        'name',
        "password",
 ]

 for (let index = 0; index < validateData.length; index++) {
    const element = validateData[index];

    if (!req.body[element]) {
        console.log('not valid data auth.')
        res.sendStatus(400)
        return
    }
    
}

const checkData = {
    name: req.body.name,
}



console.log(`find user in db. data:\n${JSON.stringify(checkData)}`)
await User.findOne(checkData,  async function(err, doc) {
    if(err) {
        res.sendStatus(500)
        return console.log(err);
    }

    if (!doc) {
        res.send('user not found')
        return
    }

    else {
    const passwordExits = encrypter.encrypt(req.body.password) == doc.password
        if (!passwordExits) {
            res.send('password not found')
            return
        }
         await TokenUser.findOne({idUser: mongoose.Types.ObjectId(doc._id)},  async function(err, docToken) {
            if(err) {
                res.sendStatus(500)
                return console.log(err);
            }

            const data = {
                idUser: mongoose.Types.ObjectId(doc._id),
                token: bencrypter.encrypt(encrypter.generate_token())
            }
            const newToken = new TokenUser(data)
          

            if (docToken) {
                


             if (check_token_date(docToken)) {
                await TokenUser.deleteOne({idUser: mongoose.Types.ObjectId(doc._id)},  async function(err, result) {
                    if(err) {
                        res.sendStatus(500)
                        return console.log(err);
                    }


                })
             }

             else {
                  const playerContainsOtherServer = await playerContainsOnServer(checkData.name)
                if  (!playerContainsOtherServer) {


                    await TokenUser.deleteOne({idUser: mongoose.Types.ObjectId(doc._id)},  async function(err, result) {
                        if(err) {
                            res.sendStatus(500)
                            return console.log(err);
                        }
    
    
                    })
                }

                else {
                res.send('token already exits')
                return                    
                }

            }

            
             
               
            }
            await  newToken.save( async function(err, doc){
                if(err) return console.log(err);
                console.log(`auth sucess. Token Data:\n${JSON.stringify(doc)} `)
                res.json({token: doc.token})
            });
         })
    }
})
 })

 router.post('/users/auth/info', async function(req, res) {
    if (!req.body.token) {
        res.sendStatus(400)
        return
    }
 
    await TokenUser.findOne({token: req.body.token},  async function(err, result) {
        
        if(err) {
            res.sendStatus(500)
            return console.log(err);
        } 
 
       if (result) {
            if (check_token_date(result)) {


                await TokenUser.deleteOne({token: result.token},  async function(err, deleteToken) {
                    if  (err) {
                        res.sendStatus(500)
                        return console.log(err);
                    } 
                  
                   res.sendStatus(400)
                   

                })
            }

            else {
                await User.findOne({_id: result.idUser},  async function(err, user) {
                    if (err) {
                        res.sendStatus(500)
                        return console.log(err);
                    } 
                      if (user) {
                          clearDataUser(user)
                          res.json(user)
                      }
        
                      else {
                        res.sendStatus(404)
                      }
                })
            }

       }
 
       else {
        
        res.sendStatus(404)
       }
       
    })
  });

router.post("/users",  async function (req, res) {
        
    if(!req.body) return res.sendStatus(400);
    
    console.log(`new user data sended:\n${JSON.stringify(req.body)}`)

    const validateData = [
        "name",
        "password",
        "age",
        "email",
    ]

    for (let index = 0; index < validateData.length; index++) {
        const element = validateData[index];

        if (!req.body[element]) {
            console.log('not valid data user.')
            res.sendStatus(400)
            return
        }
        
    }



  // check valid email input on user
   
    

    if (req.body.email) {
      if (!validatorEmail.validate(req.body.email)) {
          console.log(`Email string user not valid:\n${req.body.email}`)
          res.send('email not valid')
          return
      }
    }
    

   const data = {

    
       name: req.body.name.trim(),
       password: req.body.password.trim(),
       email: req.body.email.trim(),
       age: req.body.age
   }
    
    await User.find({name: data.name, email: data.email},  async function(err, docs){
         
        if (err) {
            res.sendStatus(500)
            return console.log(err);
        }
         
        if (docs.length > 0) {
         console.log(`user ${data.name} finded`)
         res.sendStatus(200);
         
        }
        else {
            const user = new User(data)

            user.password = encrypter.encrypt(data.password)
                
                
          await  user.save( async function(err, doc){
                if(err) return console.log(err);
                console.log(`user ${data.name} created. Date: ${doc.dateReg} `)
                res.sendStatus(201)
            });
        }

    });


});

router.post('/character',  async function (req, res) {
    if(!req.body) return res.sendStatus(400);

    
    
    console.log(`new character data sended:\n${JSON.stringify(req.body)}`)

    const validateData = [
        "name",
        "userId",
        "prefabIndex",
    ]

    for (let index = 0; index < validateData.length; index++) {
        const element = validateData[index];

        if (!req.body[element]) {
            console.log('not valid data character.')
            res.sendStatus(400)
            return
        }
        
    }
    
   const data = {
       name: req.body.name.trim(),
       userId: mongoose.Types.ObjectId(req.body.userId),
       prefabIndex: req.body.prefabIndex
   }

   await Character.find({name: data.name},  async function(err, docs) {
    if(err) {
        res.sendStatus(500)
        return console.log(err);
    } 

    if (docs.length > 0) {
        console.log(`character ${data.name} finded`)
        res.sendStatus(200);
    }

    else {
        const character = new Character(data)
        await User.findOne({_id: mongoose.Types.ObjectId(data.userId)},  async function(err, doc) {
               

        
            if(err) {
                res.sendStatus(500)
                return console.log(err);
            } 

            if (doc) {
              const  characters = doc.characters
                characters.push(mongoose.Types.ObjectId(character._id))

                const newDataUser = {
                    $set: {
                        characters: characters
                    }
                }
                doc.updateOne(newDataUser, async function(err, result) {
                    if(err) {
                        res.sendStatus(500)
                        return console.log(err);
                    } 
                   

                    await  character.save( async function(err, doc){
                        if(err) {
                            res.sendStatus(500)
                            return console.log(err);
                        } 
                        
                        console.log(`character ${data.name} created. Date: ${doc.dateReg}`)
                        res.sendStatus(201)
                    });

                });
            }

            else {
                res.sendStatus(404)
            }


        });
        
    }
   });


});


router.post('/character/info', async function (req, res) {
    if (!req.body.id || !req.body.token) {
        res.sendStatus(400)
        return
    }
 
    await TokenUser.findOne({token: req.body.token},  async function(err, docToken) {
        if  (err) {
            res.sendStatus(500)
            return console.log(err);
        } 
 
     if (docToken) {


        await User.findOne({_id: docToken.idUser},  async function(err, user) {

            if  (err) {
                res.sendStatus(500)
                return console.log(err);
            } 
            if (user) {
                await Character.findOne({_id: mongoose.Types.ObjectId(req.body.id)},  async function(err, character) {
                    if  (err) {
                        res.sendStatus(500)
                        return console.log(err);
                    }
                    
                    if (character) {
                        if (!user.characters.contains(character._id)) {
                            
                            res.sendStatus(404)
                            console.log(`user ${user.name} not have character ${character.name}`)
                        }

                        else {
                            clearDataCharacter(character)
                            res.json(character)
                        }
                    }

                    else {
                        res.sendStatus(404)
                        console.log(`character ${character.name} not found`)
                    }

                })
            }
        })
     }

     else {
        res.sendStatus(400)
     }
       
       
    })
 });


 router.post('/users/characterList', async function (req, res) {
    if (!req.body.id || !req.body.token) {
        res.sendStatus(400)
        return
    }
 
    await TokenUser.findOne({token: req.body.token},  async function(err, docToken) {
        if  (err) {
            res.sendStatus(500)
            return console.log(err);
        } 
 
     if (docToken) {


        await User.findOne({_id: docToken.idUser},  async function(err, user) {

            if  (err) {
                res.sendStatus(500)
                return console.log(err);
            } 
            if (user) {
                await Character.find({userId: mongoose.Types.ObjectId(req.body.id)},  async function(err, characters) {
                    if  (err) {
                        res.sendStatus(500)
                        return console.log(err);
                    }
                    
                    if (characters.length > 0) {
                          

                        for (let i = 0; i < characters.length; i++) {
                            const element = characters[i];
                            clearDataCharacter(element)
                        }

                            res.json(characters)

                    }

                    else {
                        res.sendStatus(404)
                    }

                })
            }
        })
     }

     else {
        res.sendStatus(400)
     }
       
       
    })
 });

 router.post('/character/status', async function (req, res) {
    if (!req.body.characterId || !req.body.token || !req.body.status) {
        res.sendStatus(400)
        return
    }

    
    req.body.status = convertToInt(req.body.status)


    await TokenUser.findOne({token: req.body.token},  async function(err, docToken) {
        if  (err) {
            res.sendStatus(500)
            return console.log(err);
        }

        if (docToken) {
            if (check_token_date(docToken)) {


                await TokenUser.deleteOne({token: docToken.token},  async function(err, deleteToken) {
                    if  (err) {
                        res.sendStatus(500)
                        return console.log(err);
                    }

                    res.sendStatus(400)
                    
                })
            }

            else {
               await User.findOne({_id: docToken.idUser},  async function(err, user) {
                if  (err) {
                    res.sendStatus(500)
                    return console.log(err);
                }


                   if (user) {
                    if (!user.characters.contains(req.body.characterId)) {
                        console.log(`user ${user.name} not have character ${req.body.characterId}`)
                        res.sendStatus(404)
                    }

                    else {
                        await Character.findOne({_id: mongoose.Types.ObjectId(req.body.characterId)},  async function(err, character) {
                            if  (err) {
                                res.sendStatus(500)
                                return console.log(err);
                            }

                            if (character) {
                                  
                                const newStatus = {
                                    $set: {
                                        online: Boolean(req.body.status)
                                    }
                                }
                                await Character.updateOne(newStatus,  async function(err, result) {
                                    if  (err) {
                                        res.sendStatus(500)
                                        return console.log(err);
                                    }

                                    res.sendStatus(204)
                                })
                            }

                            else {
                                console.log(`character (id: ${req.body.characterId}) not found`)
                                res.sendStatus(404)
                            }
    
    
                        })
                    }

                   }

                   else {
                    console.log('user not found')
                       res.sendStatus(404)
                   }
               })
            }
        }

        else {
            console.log(`token ${req.body.token} not found`)
            res.sendStatus(404)
        }
    })
 
 });


 router.post('/character/location', async function (req, res) {
    if (!req.body.characterId || !req.body.token || !req.body.location) {
        res.sendStatus(400)
        return
    }

    


    await TokenUser.findOne({token: req.body.token},  async function(err, docToken) {
        if  (err) {
            res.sendStatus(500)
            return console.log(err);
        }

        if (docToken) {
            if (check_token_date(docToken)) {


                await TokenUser.deleteOne({token: docToken.token},  async function(err, deleteToken) {
                    if  (err) {
                        res.sendStatus(500)
                        return console.log(err);
                    }

                    res.sendStatus(400)
                    
                })
            }

            else {
               await User.findOne({_id: docToken.idUser},  async function(err, user) {
                if  (err) {
                    res.sendStatus(500)
                    return console.log(err);
                }


                   if (user) {
                    if (!user.characters.contains(req.body.characterId)) {
                        console.log(`user ${user.name} not have character ${req.body.characterId}`)
                        res.sendStatus(404)
                    }

                    else {
                        await Character.findOne({_id: mongoose.Types.ObjectId(req.body.characterId)},  async function(err, character) {
                            if  (err) {
                                res.sendStatus(500)
                                return console.log(err);
                            }

                            if (character) {
                                  const dataCharacter = character.data

                                  dataCharacter.location = req.body.location


                                const newLocation = {
                                    nane: character.name,
                                    $set: {
                                       data: dataCharacter
                                    }
                                }


                                await character.updateOne(newLocation,  async function(err, result) {
                                    if  (err) {
                                        res.sendStatus(500)
                                        return console.log(err);
                                    }

                                    res.sendStatus(204)
                                })
                            }

                            else {
                                console.log(`character (id: ${req.body.characterId}) not found`)
                                res.sendStatus(404)
                            }
    
    
                        })
                    }

                   }

                   else {
                    console.log('user not found')
                       res.sendStatus(404)
                   }
               })
            }
        }

        else {
            console.log(`token ${req.body.token} not found`)
            res.sendStatus(404)
        }
    })
 
 });

 router.post('/character/valute', async function (req, res) {
    if (!req.body.characterId || !req.body.token || !req.body.anicoins) {
        res.sendStatus(400)
        return
    }

    req.body.anicoins = convertToInt(req.body.anicoins)
    req.body.gems = convertToInt(req.body.gems)
    


    await TokenUser.findOne({token: req.body.token},  async function(err, docToken) {
        if  (err) {
            res.sendStatus(500)
            return console.log(err);
        }

        if (docToken) {
            if (check_token_date(docToken)) {


                await TokenUser.deleteOne({token: docToken.token},  async function(err, deleteToken) {
                    if  (err) {
                        res.sendStatus(500)
                        return console.log(err);
                    }

                    res.sendStatus(400)
                    
                })
            }

            else {
               await User.findOne({_id: docToken.idUser},  async function(err, user) {
                if  (err) {
                    res.sendStatus(500)
                    return console.log(err);
                }


                   if (user) {
                    if (!user.characters.contains(req.body.characterId)) {
                        console.log(`user ${user.name} not have character ${req.body.characterId}`)
                        res.sendStatus(404)
                    }

                    else {
                        await Character.findOne({_id: mongoose.Types.ObjectId(req.body.characterId)},  async function(err, character) {
                            if  (err) {
                                res.sendStatus(500)
                                return console.log(err);
                            }

                            if (character) {
                                
                                const newValueValues = {
                                    nane: character.name,
                                    $set: {
                                       anicoins: req.body.anicoins,
                                       gems: req.body.gems
                                    }
                                }


                                await character.updateOne(newValueValues,  async function(err, result) {
                                    if  (err) {
                                        res.sendStatus(500)
                                        return console.log(err);
                                    }

                                    res.sendStatus(204)
                                })
                            }

                            else {
                                console.log(`character (id: ${req.body.characterId}) not found`)
                                res.sendStatus(404)
                            }
    
    
                        })
                    }

                   }

                   else {
                    console.log('user not found')
                       res.sendStatus(404)
                   }
               })
            }
        }

        else {
            console.log(`token ${req.body.token} not found`)
            res.sendStatus(404)
        }
    })
 
 });

 router.post('/character/date', async function (req, res) {
    if (!req.body.characterId || !req.body.token) {
        res.sendStatus(400)
        return
    }

    


    await TokenUser.findOne({token: req.body.token},  async function(err, docToken) {
        if  (err) {
            res.sendStatus(500)
            return console.log(err);
        }

        if (docToken) {
            if (check_token_date(docToken)) {


                await TokenUser.deleteOne({token: docToken.token},  async function(err, deleteToken) {
                    if  (err) {
                        res.sendStatus(500)
                        return console.log(err);
                    }

                    res.sendStatus(400)
                    
                })
            }

            else {
               await User.findOne({_id: docToken.idUser},  async function(err, user) {
                if  (err) {
                    res.sendStatus(500)
                    return console.log(err);
                }


                   if (user) {
                    if (!user.characters.contains(req.body.characterId)) {
                        console.log(`user ${user.name} not have character ${req.body.characterId}`)
                        res.sendStatus(404)
                    }

                    else {
                        await Character.findOne({_id: mongoose.Types.ObjectId(req.body.characterId)},  async function(err, character) {
                            if  (err) {
                                res.sendStatus(500)
                                return console.log(err);
                            }

                            if (character) {
                                  const dataCharacter = character.data

                                  dataCharacter.lastDate = dateFormat(new Date(), "dd.mm.yyyy")


                                const newLastReg = {
                                    nane: character.name,
                                    $set: {
                                       data: dataCharacter
                                    }
                                }


                                await character.updateOne(newLastReg,  async function(err, result) {
                                    if  (err) {
                                        res.sendStatus(500)
                                        return console.log(err);
                                    }

                                    res.sendStatus(204)
                                })
                            }

                            else {
                                console.log(`character (id: ${req.body.characterId}) not found`)
                                res.sendStatus(404)
                            }
    
    
                        })
                    }

                   }

                   else {
                    console.log('user not found')
                       res.sendStatus(404)
                   }
               })
            }
        }

        else {
            console.log(`token ${req.body.token} not found`)
            res.sendStatus(404)
        }
    })
 
 });

 router.post('/character/friends/add', async function (req, res) {
    if (!req.body.characterId || !req.body.token || !req.body.friend) {
        res.sendStatus(400)
        return
    }


    


    await TokenUser.findOne({token: req.body.token},  async function(err, docToken) {
        if  (err) {
            res.sendStatus(500)
            return console.log(err);
        }

        if (docToken) {
            if (check_token_date(docToken)) {


                await TokenUser.deleteOne({token: docToken.token},  async function(err, deleteToken) {
                    if  (err) {
                        res.sendStatus(500)
                        return console.log(err);
                    }

                    res.sendStatus(400)
                    
                })
            }

            else {
               await User.findOne({_id: docToken.idUser},  async function(err, user) {
                if  (err) {
                    res.sendStatus(500)
                    return console.log(err);
                }


                   if (user) {
                    if (!user.characters.contains(req.body.characterId)) {
                        console.log(`user ${user.name} not have character ${req.body.characterId}`)
                        res.sendStatus(404)
                    }

                    else {
                        await Character.findOne({_id: mongoose.Types.ObjectId(req.body.characterId)},  async function(err, character) {
                            if  (err) {
                                res.sendStatus(500)
                                return console.log(err);
                            }

                            if (character) {
                                  const dataCharacter = character.data

                                  dataCharacter.friendsList.push(mongoose.Types.ObjectId(req.body.friend))


                                const newFriendsList = {
                                    nane: character.name,
                                    $set: {
                                       data: dataCharacter
                                    }
                                }


                                await character.updateOne(newFriendsList,  async function(err, result) {
                                    if  (err) {
                                        res.sendStatus(500)
                                        return console.log(err);
                                    }

                                    res.sendStatus(204)
                                })
                            }

                            else {
                                console.log(`character (id: ${req.body.characterId}) not found`)
                                res.sendStatus(404)
                            }
    
    
                        })
                    }

                   }

                   else {
                    console.log('user not found')
                       res.sendStatus(404)
                   }
               })
            }
        }

        else {
            console.log(`token ${req.body.token} not found`)
            res.sendStatus(404)
        }
    })
 
 });


 



router.get('/characters', async function (req, res) {
    await   Character.find({}, function(err, docs){
            
        if(err) {
            res.sendStatus(500)
            return console.log(err);
        } 
   
           for (let index = 0; index < docs.length; index++) {
               clearDataCharacter(docs[index])
           }
            
   
           res.json(docs)
       });
   
   });

   router.post("/servers", async function (req, res) {
    if(!req.body) return res.sendStatus(400);
    
    console.log(`new server data sended:\n${JSON.stringify(req.body)}`)

    const data = {
        name: req.body.name
    }

    await Server.findOne({name: data.name},  async function(err, doc){
         
        if(err) {
            res.sendStatus(500)
            return console.log(err);
        } 
       
        if (doc) {
            res.send(`server ${doc.name} exits`)
            return
        }

        else {
          const server = new Server(data)
          
          await  server.save( async function(err, doc){
            if(err) {
                res.sendStatus(500)
                return console.error(err);
            } 
            console.log(`server data ${data.name} created`)
            res.sendStatus(201)
        });

        }



   });

});


router.post("/server", async function (req, res) {
    if  (!req.body.playerList || !req.body.name) {
        return res.sendStatus(400)
    }
   
    let jsonData = undefined

      try {
          jsonData = JSON.parse(req.body.playerList)

          if (!jsonData.playerList) {
              console.log(`not valid json data server schema.\nYour schema\n${JSON.stringify(jsonData)}`)
            return res.sendStatus(400)
          }
      } catch (err) {
          console.log(`JSON data server parse error ${err}`)
        return res.sendStatus(400)
        
      }
    
    console.log(`new server data sended:\n${JSON.stringify(req.body)}`)

    const data = {
        name: req.body.name
    }

    await Server.findOne({name: data.name},  async function(err, doc){
         
        if(err) {
            res.sendStatus(500)
            return console.log(err);
        } 
       
        if (doc) {
            const newDataServer = {
                $set: {
                    players: jsonData.playerList,
                    countPlayers: jsonData.playerList.length
                }
            }
          
            await Server.updateOne(newDataServer,  async function(err, result){
                if(err) {
                    res.sendStatus(500)
                    return console.log(err);
                }
                
                res.sendStatus(204) 
            })
            
        }

        else {
            console.log(`server ${req.body.name} not found`)
         res.sendStatus(404)

        }



   });

});

router.get('/servers', async function (req, res) {
    await   Server.find({}, function(err, docs){
            
        if(err) {
            res.sendStatus(500)
            return console.log(err);
        } 
   
            
   
           res.json(docs)
       });
   
   });

router.get('/servers/:name',  async function (req, res) {
    if (!req.params.name) {
        res.sendStatus(400)
    }
 
    await Server.findOne({name: req.params.name},  async function(err, doc) {
        if(err) {
            res.sendStatus(500)
            return console.log(err);
        } 
 
 
       if (!doc) {
            res.sendStatus(404)
       }
 
       else {
           res.json(doc)
       }
       
    })
})




   router.get('/count_users', async function (req, res) {
    await   User.find({}, function(err, docs){
            
        if(err) {
            res.sendStatus(500)
            return console.log(err);
        } 
   
           const data = {
            count: docs.length
        }

        res.json(data)
            
   
           
       });
   
   });

   router.get('/count_characters', async function (req, res) {
    await   Character.find({}, function(err, docs){
            
        if(err) {
            res.sendStatus(500)
            return console.log(err);
        }
   
           const data = {
               count: docs.length
           }

           res.json(data)
               
           
       });
   
   });

   router.get('/images/public/picSelectCharactersBanners/:number', async function (req, res) {
       try {
        res.sendFile( __dirname + pathPublicFolber + `${req.params.number}.png` );
       } catch  {
           res.sendStatus(404)
       }

   
   });

   router.get('/images/public/picSelectCharactersBanners', async function (req, res) {

    
       const dir = __dirname + pathPublicFolber
    try {
        fs.readdir(dir, (err, files) => {
            if(err) {
                res.sendStatus(500)
                return console.log(err);
            } 

            res.json({count: files.length});
          });
    } catch  {
        res.sendStatus(404)
    }


});





module.exports = router;