const webpack = require('webpack');
const text = require('extract-text-webpack-plugin');
const html = require('html-webpack-plugin');
const UglifyJsPlugin = require('uglifyjs-webpack-plugin')


module.exports = function(env) {
    console.log('webpack running for', env == 'prod' ? 'production' : 'development');
    var config = {};
    config.entry = {
        pollyfills: __dirname + '/wwwroot/ts/pollyfills.ts',
        vendor: __dirname + '/wwwroot/ts/vendor.ts',
        app: __dirname + '/wwwroot/ts/main.ts',
        worker: __dirname + '/wwwroot/ts/worker.ts'
    };
    config.output = {
        path: __dirname + '/wwwroot/js/',
        publicPath: '/wwwroot/js/',
        filename: '[name].js',
        sourceMapFilename: '[name].js.map'
    };
    config.resolve = {
        extensions: ['.ts', '.js']
    };
    config.module = {
        loaders: [
            {
                test: /\.ts$/,
                exclude: ['/node_modules/','/RoboHome.Site/releases/'],
                use: ['awesome-typescript-loader']
            },
            {
                test: /\.html$/,
                use: 'html-loader'
            },
            {
                test: /\.(png|jpg)$/,
                use: 'file-loader?name=assets/[name].[hash].[ext]'
            }
        ]
    };
    config.plugins = [
        new webpack.ContextReplacementPlugin(
            // The (\\|\/) piece accounts for path separators in *nix and Windows
            /angular(\\|\/)core(\\|\/)(esm(\\|\/)src|src)(\\|\/)linker/,
            __dirname + '/wwwroot/js/app', // location of your src
            {} // a map of your routes
        )
    ]
    if (env === 'prod') {
        config.plugins.push(new UglifyJsPlugin())
    } else {
        config.devtool = 'eval-cheap-module-source-map';
    }
    return config;
}