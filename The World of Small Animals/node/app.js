 const config = require('./config.js')
 const router = require('./router/router')

 const express = require('express')

 const cors = require('cors');



 const app = express()


 app.use(config.mainPage, router);
 app.use(cors())

 app.listen(process.env.PORT || config.port, () => {
    console.log(`app listening at port ${config.port}`)
  })