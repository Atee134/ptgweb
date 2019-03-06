const path = require("path");
const webpack = require('webpack');
const HtmlWebpackPlugin = require("html-webpack-plugin");
//const CleanWebpackPlugin = require("clean-webpack-plugin");
const CopyWebpackPlugin = require('copy-webpack-plugin');

module.exports = {
    entry: "./src/index.ts",
    output: {
        path: path.resolve(__dirname, "../wwwroot"),
        filename: "[name].[chunkhash].js",
        publicPath: "/"
    },
    resolve: {
        extensions: [".js", ".ts"]
    },
    module: {
        rules: [
            {
                test: /\.ts$/,
                use: "ts-loader"
            },
            {
                test: /\dynamicTerrain.js\.js$/,
                use: [ 'imports-loader?BABYLON=>require("babylonjs")' ]
            }]
    },
    plugins: [
     //   new CleanWebpackPlugin(["../wwwroot/*"], {allowExternal: true}),
        new HtmlWebpackPlugin({
            template: "./src/index.html"
        }),
        new CopyWebpackPlugin([
            {from:'src/textures',to:'textures'} 
        ]),
        new webpack.ProvidePlugin({
            'BABYLON': 'babylonjs',
          })
    ]
};