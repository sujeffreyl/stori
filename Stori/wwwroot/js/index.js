(function() {
  var appid = "D41D8CD98F00B204E9800998ECF8427E1510C791";
  var path = "https://www.bingapis.com/api/v7/images/visualsearch?appId=" + appid;
    
  var width = 0;    // We will scale the photo width to this
  var height = $(window).height();     // This will be computed based on the input stream

  var streaming = false;

  var video = null;
  var canvas = null;
  var startbutton = null;
  var photo = null;

  function startup() {
    video = document.getElementById('video');
    canvas = document.getElementById('canvas');
    camera = document.getElementById('camera');
    photo = document.getElementById('photo');
    startbutton = document.getElementById('shutter');

    navigator.getMedia = ( navigator.getUserMedia ||
                           navigator.webkitGetUserMedia ||
                           navigator.mozGetUserMedia ||
                           navigator.msGetUserMedia);

    navigator.getMedia(
      {
        video: true,
        audio: false
      },
      function(stream) {
        if (navigator.mozGetUserMedia) {
          video.mozSrcObject = stream;
        } else {
          var vendorURL = window.URL || window.webkitURL;
          video.src = vendorURL.createObjectURL(stream);
        }
        video.play();
      },
      function(err) {
        console.log("An error occured! " + err);
      }
    );

    video.addEventListener('canplay', function(ev){
      if (!streaming) {
        width = video.videoWidth / (video.videoHeight/height);
      
        // Firefox currently has a bug where the height can't be read from
        // the video, so we will make assumptions if this happens.
      
        if (isNaN(height)) {
          width = height / (4/3);
        }
      
        video.setAttribute('width', width);
        video.setAttribute('height', $(window).height());
        canvas.setAttribute('width', width);
        canvas.setAttribute('height', $(window).height());
        $('.camera').first().css('left', - width / 3);
        streaming = true;
      }
    }, false);

    startbutton.addEventListener('click', function(ev){
      takepicture();
      ev.preventDefault();
    }, false);
    
  }

  function takepicture() {
    var context = canvas.getContext('2d');
    if (width && height) {
      canvas.width = $(window).width();
      canvas.height = height;
      context.drawImage(video, - width / 3, 0, width, height);
    
      var data = canvas.toDataURL("image/jpeg");
      photo.setAttribute('src', data);
      data = data.substr(data.indexOf(',')+1)
      
      var request = new XMLHttpRequest();
      request.open('POST', path, true);

      request.onload = function () {
        if (request.status >= 200 && request.status < 300) {
          console.log(JSON.parse(request.responseText));
        }
        else {
          console.log(request.responseText);
        }
      }
      
      var formData = new FormData();
      formData.append('imageBase64', data); // the first parameter sets the name and is required
      request.send(formData);
    }
  }

  window.addEventListener('load', startup, false);
})();

