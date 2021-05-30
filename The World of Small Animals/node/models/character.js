const configCharacter = require('../others/character_config')
const dateFormat = require('dateformat');
const mongoose = require("mongoose");
const config = require('../crypto/crypto_config');
const Schema = mongoose.Schema;

const characterSchema = new Schema({

    
    versionKey: false,


    name: {
        type: String,
        required: true,
        minlength: 1,
    },

    userId: {
        type: mongoose.Types.ObjectId,
        required: true,
        minlength: 1
    },

    dateReg: {
        type: String,
        default: dateFormat(new Date(), "dd.mm.yyyy"),
    },


    anicoins: {
      type: Number,
      default: configCharacter.anicoinsDefault,
      minlength: 1
    },

    gems: {
        type: Number,
        default: configCharacter.gemsDefault,
        minlength: 1
      },

      online: {
          type: Boolean,
          default: false
      },

      prefabIndex: {
          type: Number,
          required: true,
          default: 0
      },

      data: {

          friendsList: {
              type: [mongoose.Types.ObjectId]
          },

          location: {
            type: String,
            default: ''
        },


          gifts: [Object],
          lastDate: {
              type: String,
              default: dateFormat(new Date(), "dd.mm.yyyy"),
          },

         level: {
             currentLevel: {
                 type: Number,
                 default: configCharacter.levelDefault
             },
             xpNext: {
                 type: Number,
                 default: configCharacter.xpToNextLevelDefault
             },

             xpCurrent: {
                 type: Number,
                 default: 0
             }
         }
      }
}, {versionKey: false})

module.exports = characterSchema;