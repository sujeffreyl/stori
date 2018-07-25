(function() {
  var appid = "D41D8CD98F00B204E9800998ECF8427E1510C791";
  var path = "https://www.bingapis.com/api/v7/images/visualsearch?appId=" + appid;
    
  var width = 0;    // We will scale the photo width to this
  var height = $(window).height();     // This will be computed based on the input stream

  var streaming = false;

  var video = null;
  var canvas = null;
  var startbutton = null;
  var closebutton = null;
  var photo = null;
  var tags = null;

  function startup() {
    video = document.getElementById('video');
    canvas = document.getElementById('canvas');
    camera = document.getElementById('camera');
    photo = document.getElementById('photo');
    tags = document.getElementById('tags');
    startbutton = document.getElementById('shutter');
    closebutton = document.getElementById('close');

    navigator.getMedia = ( navigator.getUserMedia ||
                           navigator.webkitGetUserMedia ||
                           navigator.mozGetUserMedia ||
                           navigator.msGetUserMedia);

    navigator.getMedia(
    {
        video: {
            facingMode: { exact: "environment" }
        },
        // true,
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
        // $('.camera').first().css('left', - width / 3);
        streaming = true;
      }
    }, false);

    startbutton.addEventListener('click', function(ev){
      takepicture();
      ev.preventDefault();
    }, false);

    closebutton.addEventListener('click', function(ev){
      $('#photo').removeAttr('src');
      $('#tags').css('display', 'none');
      $('.output').first().css('display', 'none');
      ev.preventDefault();
    }, false);
    
    }

    function getTags(json) {
        var tagString = '';
        $('.image', '#tags').remove(); 
        for (var i = 1; i < Math.min(json["tags"].length, 3); i++) {
            tagString += json["tags"][i].displayName + '{ ';
            var displayName = json["tags"][i].displayName;
            console.log(displayName)
            for (var j = 0; j < json["tags"][i]["actions"].length; j++) {
                tagString += json["tags"][i]["actions"][j].actionType + ', '
            }
            tagString += '}, ';

            if (!displayName.startsWith('##')) {
                var img = "https://www.bing.com/th?q=" + displayName + "&dc=1&w=100&h=100&c=1&dpr=2&mkt=en-US&adlt=moderate&t=1";
                $('#tags').append("<a class='image' href='/search?q=" + displayName + "'><img src='" + img + "'></a>");
            }
        }
        return tagString;
    }

  function takepicture() {
    var context = canvas.getContext('2d');
    if (width && height) {
      canvas.width = $(window).width();
      canvas.height = height;
      context.drawImage(video, 0, 0, width, height);
    
      var data = canvas.toDataURL("image/jpeg");
      photo.setAttribute('src', data);
      data = data.substr(data.indexOf(',')+1)
    }

    $('.output').first().css('display', 'block');

    var request = new XMLHttpRequest();
    request.open('POST', path, true);

    request.onload = function () {
        if (request.status >= 200 && request.status < 300) {
            var json = JSON.parse(request.responseText);
            console.log(json);
            // document.getElementById('json').innerHTML = getTags(json);
            getTags(json);
            $('#tags').css('display', 'block');
        }
        else {
            console.log(request.responseText);
        }
    }

    var formData = new FormData();
    if (data) {
        formData.append('imageBase64', data); // the first parameter sets the name and is required
    } else {
        formData.append('knowledgeRequest', "{'imageInfo': {'url': 'https://ae01.alicdn.com/kf/HTB10dQILVXXXXX3XFXXq6xXFXXXx/Creative-Resin-Planter-Flowerpot-Kawaii-Corgi-Garden-Succulent-Plants-Jardin-Bonsai-Desk-font-b-Flower-b.jpg'}}");
    }
    request.send(formData);
  }

  window.addEventListener('load', startup, false);
})();

