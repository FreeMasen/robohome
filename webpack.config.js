const webpack = require('webpack');
const text = require('extract-text-webpack-plugin');
const html = require('html-webpack-plugin');
const sass = require('node-sass');


module.exports = function(env) {
    compileCss(env);
    var config = {};
    config.entry = {
        pollyfills: __dirname + '/RoboHome.Site/wwwroot/ts/pollyfills.ts',
        vendor: __dirname + '/RoboHome.Site/wwwroot/ts/vendor.ts',
        app: __dirname + '/RoboHome.Site/wwwroot/ts/main.ts',
        worker: __dirname + '/RoboHome.Site/wwwroot/ts/worker.ts'
    };
    config.output = {
        path: __dirname + '/RoboHome.Site/wwwroot/js/',
        publicPath: '/RoboHome.Site/wwwroot/js/',
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
                exclude: ['/RoboHome.Site/node_modules/','/RoboHome.Site/releases/'],
                use: ['awesome-typescript-loader']
            },
            {
                test: /\.css$/,
                exclude: __dirname + '/RoboHome.Site/wwwroot/js/app',
                use: text.extract({ fallback: 'style-loader', use: 'css-loader?sourceMap' })
            },
            {
                test: /\.css$/,
                include: __dirname + '/RoboHome.Site/wwwroot/js/app',
                use: 'raw-loader'
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

    } else {
        config.devtool = 'eval-cheap-module-source-map';
    }
    return config;
}

function compileCss(env) {
    console.log('Compiling css for ', env);
    var config = {};
    config.file = __dirname + '/RoboHome.Site/wwwroot/sass/style.scss';
    config.outFile = __dirname + '/RoboHome.Site/wwwroot/css/style.css'
    if (env === 'prod') {
        config.outputStyle = 'compressed';
    } else {
        config.outputStyle = 'expanded';
        config.sourceMap = true;
    }
    
    var css = sass.renderSync(config);
    var fs = require('fs');
    fs.writeFileSync(config.outFile, css.css);
    if (config.sourceMap) {
        fs.writeFileSync(config.outFile + '.map', css.map);
    }
    console.log('Compiling complete');
}