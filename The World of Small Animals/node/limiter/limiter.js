const config = require('./limiter_config')

const rateLimit = require("express-rate-limit");


const limiter = rateLimit({
    windowMs: config.timeMilliseconds, // max time wait request
    max: config.maxRequestCount // limit each IP to N requests per windowMs
  });
module.exports = limiter