var cart = {
    init: function () {
        cart.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();


        });
    }
}

cart.init();