  
const config = require('./crypto_config')

const shordid = require('shortid')


var crypto = require('crypto'),
    algorithm =  config.algoritme,
    password = config.key;


function encrypt(text){
  var cipher = crypto.createCipher(algorithm,password)
  var crypted = cipher.update(text, config.encoding, config.inputEncoding)
  crypted += cipher.final(config.inputEncoding);
  return crypted;
}
 
function decrypt(text){
  var decipher = crypto.createDecipher(algorithm,password)
  var dec = decipher.update(text,config.inputEncoding, config.encoding)
  dec += decipher.final(config.encoding);
  return dec;
}

function generate_token() {
return crypto.randomBytes(64).toString('hex');
}

module.exports = {
    encrypt,
     decrypt,
     generate_token
}