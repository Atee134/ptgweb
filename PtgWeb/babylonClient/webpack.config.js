const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const CleanWebpackPlugin = require("clean-webpack-plugin");
const CopyWebpackPlugin = require('copy-webpack-plugin');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

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
        ]
    },
    plugins: [
        new CleanWebpackPlugin(["../wwwroot/*"], {allowExternal: true}),
        new HtmlWebpackPlugin({
            template: "./src/index.html"
        }),
        new CopyWebpackPlugin([
            {from:'src/textures',to:'textures'} 
        ])
    ]
};