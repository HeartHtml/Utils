#!/usr/bin/env node

var path = require('path')
var fs = require('fs')
var winston = require('winston')
var rimraf = require('rimraf')
var mv = require('mv')
var moment = require('moment')
var ArgumentParser = require('argparse').ArgumentParser
var parser = new ArgumentParser({
  version: '1.0.0',
  addHelp: true,
  description: 'Media Import Organizer'
})
parser.addArgument(
  [ '-d', '--dir' ],
  {
    help: 'Recursively organizes all of the media in this folder and splits pictures and videos into respective folders'
  }
)

var photoExtensions = [/\.png$/,
  /\.PNG$/,
  /\.jpg$/,
  /\.JPG$/,
  /\.jpeg$/,
  /\.JPEG$/]
var movieExtensions = [/\.mov$/,
  /\.MOV$/,
  /\.mp4$/,
  /\.MP4$/,
  /\.wmv$/,
  /\.WMV$/,
  /\.MKV$/,
  /\.mkv$/,
  /\.avi$/,
  /\.AVI$/]

var args = parser.parseArgs()

module.exports = organizeFolder(args.dir)

function organizeFolder (dir) {
  if (!dir) {
    parser.printHelp()
  } else {
    if (!fs.existsSync(dir)) {
      winston.info('Directory does not exist', dir)
      return
    }
    winston.info(`Will organize files in directory ${dir}`)
    var cleanUpDir = path.join(dir, 'miorgPhotos')
    rimraf.sync(cleanUpDir)
    cleanUpDir = path.join(dir, 'miorgMovies')
    rimraf.sync(cleanUpDir)
    var photos = { type: 'Photos', outputDirName: 'miorgPhotos', files: [] }
    var videos = { type: 'Movies', outputDirName: 'miorgMovies', files: [] }
    photoExtensions.map((extension) => {
      fromDir(dir, extension, (filename) => {
        winston.info('found photo:', filename)
        photos.files.push(filename)
      })
    })
    movieExtensions.map((extension) => {
      fromDir(dir, extension, (filename) => {
        winston.info('found video:', filename)
        videos.files.push(filename)
      })
    })
    if (photos.files.length > 0) {
      organizeMediaType(dir, photos)
    } else {
      winston.info('No photos found...')
    }
    if (videos.files.length > 0) {
      organizeMediaType(dir, videos)
    } else {
      winston.info('No movies found...')
    }
  }
}

function organizeMediaType (baseDirectory, mediaDictionary) {
  winston.info('organizing media:', mediaDictionary.type)
  var mediaDir = path.join(baseDirectory, mediaDictionary.outputDirName)
  var filesByDateTaken = []
  if (!fs.existsSync(mediaDir)) {
    fs.mkdirSync(mediaDir)
  }
  winston.info('gathering file metadata:', mediaDictionary.type)
  mediaDictionary.files.map((file) => {
    var stat = fs.lstatSync(file)
    filesByDateTaken.push({fileName: file, dateTaken: stat.mtime})
  })
  var datesTaken = []
  winston.info('grouping by date taken:', mediaDictionary.type)
  filesByDateTaken.map((fileByDateTaken) => {
    var elementFound = datesTaken.find((element) => { fileByDateTaken.dateTaken === element })
    if (!elementFound) {
      datesTaken.push(fileByDateTaken.dateTaken)
    }
  })
  winston.info('moving files:', mediaDictionary.type)
  datesTaken.map((dateTaken) => {
    var m = moment(dateTaken)
    var dirName = m.format('YYYY-MM-DD')
    var dateTakenDir = path.join(mediaDir, dirName)
    if (!fs.existsSync(dateTakenDir)) {
      fs.mkdirSync(dateTakenDir)
    }
    var filesWithDateTaken = filesByDateTaken.filter((element) => element.dateTaken === dateTaken)
    filesWithDateTaken.map((file) => {
      var existingFileName = path.parse(file.fileName).base
      var newPath = path.join(dateTakenDir, existingFileName)
      winston.info('Moving ' + file.fileName + ' to ' + newPath)
      mv(file.fileName, newPath, function (err) {
        if (err) {
          winston.error('Something went wrong moving the source file: ' + err)
        }
      })
    })
  })
}

function fromDir (startPath, filter, callback) {
  var files = fs.readdirSync(startPath)
  for (var i = 0; i < files.length; i++) {
    var filename = path.join(startPath, files[i])
    var stat = fs.lstatSync(filename)
    if (stat.isDirectory()) {
      fromDir(filename, filter, callback) // recurse
    } else if (filter.test(filename)) callback(filename)
  };
};

// fromDir('../LiteScript', /\.html$/, function (filename) {
//   console.log('-- found: ', filename)
// })
