'use strict'

const config = require('../others/tokenUser_config')

const encrypter = require('../crypto/encypter')
const mongoose = require("mongoose");

const dateFormat = require('dateformat');


Date.prototype.addDays = function(days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
}


const Schema = mongoose.Schema;

const TokenUser = new Schema({
    idUser: {
        type: mongoose.Types.ObjectId,
        required: true
    },

    time: {
        type: Date,
        default:  new Date().addDays(config.days)
    },

    token: {
        type: String,
        required: true
    }
}, {versionKey: false})

module.exports = TokenUser;