mergeInto(LibraryManager.library, {
  ReturnString: function(string){
    var bufferSize = lengthBytesUTF8(string)+1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(string, buffer, bufferSize);
    return buffer;
  },
  IsHandheld: function(){
    return true;
    /*var userAgent = navigator.userAgent || navigator.vendor || window.opera;
    if (/android/i.test(userAgent) || (/iPad|iPhone|iPod/.test(userAgent) && !window.MSStream)) {
      console.log('Movile platform');
      return true;
    } else {
      console.log('Desktop platform');
      return false;
    }*/
  }
});
