(function() {
  var sb = null;

  function init() {
    sb = document.querySelector('#searchbox > input');

    sb.addEventListener('keypress', function(evt){
        if (evt.which == 13 || evt.keyCode == 13) {
            window.location.replace("/search?q=" + sb.value);
        }
    }, false);
  }

  window.addEventListener('load', init, false);
})();

