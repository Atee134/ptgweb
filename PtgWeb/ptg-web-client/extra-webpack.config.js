const webpack = require('webpack');
const path = require('path');

module.exports = {
    resolve: {
        extensions: ['.ts', '.js']
    },
    module: {
        rules: [{
            test:/\.js$/,
            include: [
                path.resolve(__dirname, "./src/app/game/extensions")
            ],
            use: [ 'imports-loader?BABYLON=>require("babylonjs")' ]
        }]
    },
    plugins: [
        new webpack.ProvidePlugin({
          'BABYLON': 'babylonjs'
        })
    ]
}