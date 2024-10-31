mergeInto(LibraryManager.library, {
    OpenURLInAnotherWindow: function(link){
        window.open("window.open(\"" + link + "\")");
    },
  });