var sass = require('node-sass');
var fs = require('fs');
var path = require('path');

function watch(env) {
    var sassRoot = __dirname + '/wwwroot/sass/';
    var cssRoot = __dirname + '/wwwroot/css/';
    console.log('watching sass files');
    fs.watch(sassRoot, (event, fn) => {
        console.log('Compiling css for ', fn);
        var config = {};
        config.file = sassRoot + fn;
        config.outFile = cssRoot + fn.replace('.scss', '.css');
        if (env === 'prod') {
            config.outputStyle = 'compressed';
        } else {
            config.outputStyle = 'expanded';
            config.sourceMap = true;
        }
        renderCss(config);
    });
}

function renderCss(config) {
    sass.render(config, (err, result) => {
        var fn = path.basename(config.file);
        if (err) return console.error(err.message);
        console.log('Compiled ', fn);
        fs.writeFile(config.outFile, result.css, err => {
            if (err) return console.error(err.message);
            console.log('Completed compiling css for', fn)
        });
        if (config.sourceMap) {
            fs.writeFile(config.outFile + '.map', result.map, err => {
                if (err) return console.error(err.message);
                console.log('Wrote map for ', fn);
            });
        }
    });
}

var arg = process.argv[0];

if (!arg) arg = 'dev';

watch(arg);
