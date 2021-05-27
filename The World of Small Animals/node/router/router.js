'use strict'

const cors = require('cors');


const config = require('../config.js')
const encrypter = require('../crypto/encypter')
const mongoose = require("mongoose");
const express = require('express');

const helmet = require('helmet')

const bodyParser = require('body-parser');
const  validatorEmail = require("email-validator");



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


router.delete('/users/:name', async function (req, res) {
    if (!req.params.name) {
        res.sendStatus(500)
        return
    }
 
    await User.deleteOne({name: req.params.name},  async function(err, result) {
        
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

 router.delete('/users/auth/token/:token', async function (req, res) {
    if (!req.params.token) {
        res.sendStatus(500)
        return
    }
 
    await TokenUser.deleteOne({token: req.params.token},  async function(err, result) {
        
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

 router.get('/users/auth/:token', async function (req, res) {
    if (!req.params.token) {
        res.sendStatus(500)
        return
    }
 
    await TokenUser.findOne({token: req.params.token},  async function(err, doc) {
        
        if(err) {
            res.sendStatus(500)
            return console.log(err);
        } 

       if  (doc) {
        await User.findOne({_id: mongoose.Types.ObjectId(),  async function(err, doc) {
            if (doc) {
                res.json(doc)
            }

            else {
                res.sendStatus(404)
            }
        }
       })
    }

    else {
        res.sendStatus(404)
    }
    
    })
 })

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
                token: encrypter.generate_token()
            }
            const newToken = new TokenUser(data)
          

            if (docToken) {
                const dateNow = new Date()
                const dateToken = docToken.time
                 const conditions = dateToken.getDay() != dateNow.getDay() + 1 || dateToken.getMonth() != dateNow.getMonth() ||  dateToken.getFullYear() != dateNow.getFullYear()


             if (conditions) {
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
            await  newToken.save( async function(err, doc){
                if(err) return console.log(err);
                console.log(`auth sucess. Token Data:\n${JSON.stringify(doc)} `)
                res.json({token: doc.token})
            });
         })
    }
})
 })

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
                characters.push(mongoose.Types.ObjectId(data.userId))

                const newDataUser = {
                    _id: doc._id,
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

router.get('/character/:name',  async function (req, res) {
    if (!req.params.name) {
        res.sendStatus(500)
        return
    }
 
    await Character.findOne({name: req.params.name},  async function(err, doc) {
        if(err) {
            res.sendStatus(500)
            return console.log(err);
        } 
 
 
       if (!doc) {
            res.sendStatus(404)
       }
 
       else {
           clearDataCharacter(doc)
           res.json(doc)
       }
       
    })
})

router.delete('/character/:name', async function (req, res) {
    if (!req.params.name) {
        res.sendStatus(500)
        return
    }
 
    await Character.deleteOne({name: req.params.name},  async function(err, result) {
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

    await User.findOne({name: data.name},  async function(err, doc){
         
        if(err) {
            res.sendStatus(500)
            return console.log(err);
        } 
       
        if (doc) {
            res.sendStatus(500)
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



module.exports = router;