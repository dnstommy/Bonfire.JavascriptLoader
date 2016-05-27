'use strict';

var gulp = require('gulp');
var inject = require('gulp-inject');
var uglify = require('uglify-js');

var ASSET_PATH = './src/Bonfire.JavascriptLoader/Assets';
var INJECT_PATH = './src/Bonfire.JavascriptLoader/Core';

gulp.task('build', function() {
  gulp.src(INJECT_PATH + '/JavascriptLoaderHtmlHelper.cs')
    .pipe(inject(gulp.src(ASSET_PATH + '/loader.js'), {
      starttag: '/*INJECT:JS*/',
      endtag: '/*ENDINJECT*/',
      transform: function(filepath, file) {
        return '"' + uglify.minify(file.contents.toString('utf8'), { fromString: true }).code.slice(1) + '"';
      }
    }))
    .pipe(gulp.dest(INJECT_PATH));
});

gulp.task('default', ['build']);
