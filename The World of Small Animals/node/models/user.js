const mongoose = require("mongoose");
const dateFormat = require('dateformat');



const Schema = mongoose.Schema;

const userScheme = new Schema({
    name: {
        type: String,
        required: true,
        minlength: 1,
    },

      password: {
        type: String,
        required: true,
        minlength: 6,
    },


    age: {
        type: Number,
        required: true,
        minlength: 1
    },

    
    email: {
        type: String,
        required: true,
        minlength: 1,
    },
    dateReg: {
        type: String,
        default: dateFormat(new Date(), "dd.mm.yyyy"),
    },

    characters: {
        type: Array
    },
    role: {
        type: String,
        default: 'user'
    }
});

module.exports = userScheme