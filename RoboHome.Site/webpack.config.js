const webpack = require('webpack');
const UglifyJsPlugin = require('uglifyjs-webpack-plugin')
const path = require('path');

module.exports = function(env) {
    let outDir = path.join(__dirname, 'wwwroot', 'js');
    let devtool = false;
    let envDesc;
    let minify = false;
    switch (env) {
        case 'prod':
            envDesc = 'production';
            minify = true;
        break;
        case 'js-only':
            outDir = path.join(__dirname, 'bin', 'Release', 'netcoreapp2.0', 'publish', 'wwwroot', 'js');
            envDesc = 'production (js-only)';
            minify = true;
        break;
        default:
            devtool = 'source-map';
        break;
    }
    console.log('webpack running for', envDesc);
    var config = {};
    config.entry = {
        pollyfills: __dirname + '/wwwroot/ts/pollyfills.ts',
        vendor: __dirname + '/wwwroot/ts/vendor.ts',
        app: __dirname + '/wwwroot/ts/main.ts',
        worker: __dirname + '/wwwroot/ts/worker.ts'
    };
    config.output = {
        path: outDir,
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
    if (minify) {
        config.plugins.push(new UglifyJsPlugin())
    }
    config.devtool = devtool;
    return config;
}