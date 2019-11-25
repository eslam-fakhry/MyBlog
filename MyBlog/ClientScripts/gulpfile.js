'use strict'

var gulp = require('gulp')
var sass = require('gulp-sass')
var concat = require('gulp-concat');

sass.compiler = require('node-sass')

gulp.task('sass:dev', function () {
    return gulp.src( './src/sass/main.scss' )
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('../wwwroot/dist/css/'))
})

gulp.task('js:dev', function(){
    return gulp.src( [ 
        'node_modules/jquery/dist/jquery.js',
        'node_modules/bootstrap/dist/js/bootstrap.bundle.js',
        './src/js/app.js' ] )
        .pipe(concat('bundle.js'))
        .pipe(gulp.dest('../wwwroot/dist/js/'))
})

gulp.task('dev',gulp.series(['sass:dev','js:dev']))

gulp.task('sass:watch', function () {
    gulp.watch('./src/sass/*.scss', gulp.series(['sass:dev']))
})

gulp.task('js:watch', function () {
    gulp.watch('./src/js/app.js', gulp.series(['js:dev']))
})

gulp.task('watch',function(){
    gulp.watch('./src/sass/*.scss', gulp.series(['sass:dev']))
    gulp.watch('./src/js/app.js', gulp.series(['js:dev']))
})
