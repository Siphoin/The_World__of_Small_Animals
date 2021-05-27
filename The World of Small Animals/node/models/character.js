const configCharacter = require('../others/character_config')
const dateFormat = require('dateformat');
const mongoose = require("mongoose");
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

          location: String,
          friendsList: [String],
          gifts: [Object],
          lastDate: {
              type: String,
              default: dateFormat(new Date(), "dd.mm.yyyy"),
          }
      }
}, {versionKey: false})

module.exports = characterSchema;