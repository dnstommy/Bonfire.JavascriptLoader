'use strict';

import gulp from 'gulp';
import ignite from 'gulp-ignite';
import browserify from 'gulp-ignite-browserify';
import babelify from 'babelify';
import inject from 'gulp-inject';
import uglify from 'uglify-js';
import yargs from 'yargs';

const ASSET_PATH = './src/Bonfire.JavascriptLoader/Assets';
const INJECT_PATH = './src/Bonfire.JavascriptLoader/Core';

const buildTask = {
  name: 'build',
  fn() {
    return gulp.src(INJECT_PATH + '/JavascriptLoaderEnvironment.cs')
      .pipe(inject(gulp.src(`${ASSET_PATH}/loader.js`), {
        starttag: '/*INJECT:JS*/',
        endtag: '/*ENDINJECT*/',
        transform: (filepath, file) => {
          return `"${uglify.minify(file.contents.toString('utf8'), { fromString: true }).code.slice(1)}"`;
        }
      }))
      .pipe(gulp.dest(INJECT_PATH));
  }
}

const tasks = [
  browserify,
  buildTask,
];

const filename = yargs.argv.filename || yargs.argv.f || 'client.js';

const options = {
  browserify: {
    src: `./src/Bonfire.JavascriptLoader.Demo/Assets/${filename}`,
    dest: './src/Bonfire.JavascriptLoader.Demo/Content/js',
    filename: filename,
    options: {
      transform: [babelify],
      standalone: filename === 'client.js' ? false : '__JavascriptLoader',
    },
    watchFiles: [
      './src/Bonfire.JavascriptLoader.Demo/Assets/*',
    ],
  },
};

ignite.start(tasks, options);
