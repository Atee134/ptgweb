const webpack = require('webpack');

module.exports = {
    module: {
        rules: [{
            test: /\dynamicTerrain.js\.js$/,
            use: [ 'imports-loader?BABYLON=>require("babylonjs")' ]
        }]
    },
    plugins: [
        new webpack.ProvidePlugin({
          'BABYLON': 'babylonjs'
        })
    ]
}