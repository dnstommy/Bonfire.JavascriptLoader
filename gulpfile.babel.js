import gulp from 'gulp';
import ignite from 'gulp-ignite';
import browserify from 'gulp-ignite-browserify';
import babelify from 'babelify';
import inject from 'gulp-inject';
import uglify from 'uglify-js';
import yargs from 'yargs';
import Nuget from 'nuget-runner';
import msbuild from 'gulp-msbuild';
import del from 'del';
import assemblyInfo from 'gulp-dotnet-assembly-info';
import { IGNITE_UTILS } from 'gulp-ignite/utils';
import pkg from './package.json';

const SRC_PATH = './src/Bonfire.JavascriptLoader';
const DEMO_PATH = './src/Bonfire.JavascriptLoader.Demo';
const version = pkg.version;

const assembly = {
  name: 'assembly-info',
  fn() {
    return gulp
      .src(`${SRC_PATH}/Properties/AssemblyInfo.cs`)
      .pipe(assemblyInfo({
        title: 'Bonfire.JavascriptLoader',
        description: 'Enable javascript loading for component based architecture.',
        product: 'Bonfire.JavascriptLoader',
        fileVersion: version,
        company: 'Bonfire, Inc.',
        copyright: `Copyright © ${new Date().getFullYear()}`,
        version,
      }))
      .pipe(gulp.dest(`${SRC_PATH}/Properties`));
  },
};

const build = {
  name: 'build',
  fn() {
    return gulp
      .src(`${SRC_PATH}/Bonfire.JavascriptLoader.csproj`)
      .pipe(msbuild({
          toolsVersion: 14.0,
          targets: ['Clean', 'Build'],
          errorOnFail: true,
          stdout: false,
          configuration: yargs.argv.config || 'Release'
      }));
  },
}

const injectLoaderJS = {
  name: 'inject-loaderjs',
  fn() {
    return gulp.src(`${SRC_PATH}/Core/JavascriptConfiguration.cs`)
      .pipe(inject(gulp.src(`${SRC_PATH}/Assets/loader.js`), {
        starttag: '/*INJECT:JS*/',
        endtag: '/*ENDINJECT:JS*/',
        transform: (filepath, file) => {
          return `"${uglify.minify(file.contents.toString('utf8'), { fromString: true }).code.slice(1)}"`;
        }
      }))
      .pipe(gulp.dest(`${SRC_PATH}/Core`));
  }
};

const nugetPack = {
  name: 'nuget-pack',
  fn(options, end) {
    gulp.src(`${SRC_PATH}/bin/Release/Bonfire.JavascriptLoader.dll`)
      .pipe(gulp.dest('./build/nuget/lib/net45'))
        .on('end', () => {
          const nuget = Nuget();

          nuget.pack({
            spec: './build/nuget/JavascriptLoader.nuspec',
            outputDirectory: 'build/',
            version,
          }).then(() => end());
        });
  }
};

const nugetPush = {
  name: 'nuget-push',
  fn() {
    const nuget = Nuget();

    nuget.push(`./build/Bonfire.JavascriptLoader.${version}.nupkg`, {
      source: 'https://www.myget.org/F/bonfire/api/v2/package',
      timeout: 600,
      apiKey: '83e4043c-0b7b-4d4f-b0ce-4f8918792dc7',
      verbosity: 'normal',
    });
  }
}

const publish = {
  name: 'publish',
  run: ['assembly-info', 'inject-loaderjs', 'build', 'nuget-pack', 'nuget-push'],
};

const tasks = [
  assembly,
  nugetPack,
  browserify,
  injectLoaderJS,
  build,
  publish,
  nugetPush,
];

const options = {
  browserify: {
    src: `${DEMO_PATH}/Assets/main.js`,
    dest: `${DEMO_PATH}/Content/js`,
    filename: 'main.js',
    options: {
      transform: [babelify],
    },
    watchFiles: [
      `${DEMO_PATH}/Assets/*`,
    ],
  },
};

ignite.start(tasks, options);
