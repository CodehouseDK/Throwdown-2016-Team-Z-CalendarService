var path = require("path");
var webpack = require("webpack");
var ExtractTextPlugin = require("extract-text-webpack-plugin");


module.exports = {
    output: {
        path: path.resolve(__dirname, "wwwroot/scripts"),
        filename: "bundle.js",

    },
    entry: {
        app: path.resolve(__dirname, "wwwroot/scripts/app.js")
    },
    module: {
        loaders: [
            {
                test: /\.js$/,
                exclude: /node_modules/,
                loader: "babel",
                query: {
                    presets: ['es2015'],
                    compact: false
                }
            },
            {
                test: /\.(scss|sass|css)$/,
                loader: ExtractTextPlugin.extract("style", "css!sass")
            },
            {
                test: /\.hbs$/,
                loader: "handlebars-loader"
            },
            {
                test: /\.html/,
                loader: "html"
            },

            {
                test: /\.(woff|woff2|eot|ttf|jpe?g|png|gif|svg)(\?v=[0-9]\.[0-9]\.[0-9])?$/,
                loader: 'file-loader?name=../../wwwroot/images/[hash].[ext]'
            }
        ]
    },
    plugins: [
        new ExtractTextPlugin("../../wwwroot/style/bundle.css"),
        new webpack.DefinePlugin({ 'require.specified': 'require.resolve' }),
        
    ]
};