const mongoose = require("mongoose");
const Schema = mongoose.Schema;

const serverSchema = new Schema({
    name: {
        type: String,
        required: true,
        default: 'Server_Name',
    },
    countPlayers: {
        type: Number,
        default: 1
    }
}, {versionKey: false})

module.exports = serverSchema;