$(window).unload(function () {
    if ($.browser.msie && $.browser.version < 8) {
        for (var id in jQuery.cache) {
            if (jQuery.cache[id].handle) {
                try {
                    jQuery.event.remove(jQuery.cache[id].handle.elem);
                } catch (e) { }
            }
        }
        window.detachEvent("onload", jQuery.ready);        
    }
});