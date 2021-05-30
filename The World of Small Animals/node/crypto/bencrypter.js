const config = require('./bcrypto_config')


const bcrypt = require('bcrypt');
 
const salt = bcrypt.genSaltSync(config.saitSize);
 

function encrypt(text) {
  return  bcrypt.hashSync(text, salt)
}

module.exports = {
    encrypt
}