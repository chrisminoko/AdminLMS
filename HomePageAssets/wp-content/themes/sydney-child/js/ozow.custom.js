jQuery(document).ready(function ($) {


    //Blog font icon
    $('.dropdown').click(function () {
        $(this).find('i').toggleClass('fa-angle-up fa-angle-down')
    });

    // icon replace
    $('i.fa-times').toggleClass('fa fa-times la la-times');
    $('i.fa-plus').toggleClass('fa fa-plus la la-plus');
    $('i.fa-minus').toggleClass('fa fa-minus la la-minus');

    // ------------------ Counter section --------------------
    $('.counter').show();
    $('.counter').counterUp({
        delay: 20,
        time: 3000
    });

    let startCount = parseInt($("#counderHidden").val());
    let CountSpeed = 5000;
    startCounter();
    setInterval(function () {
        startCounter();
    }, CountSpeed);

    function startCounter() {
        $(".counter").html(startCount.toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        startCount += 115;
    }

    // ------------------ Counter section end --------------------


    // ------------------ Carousel initialize --------------------
    $(".carousel").swipe({
        swipe: function (event, direction, distance, duration, fingerCount, fingerData) {
            if (direction == 'left') $(this).carousel('next');
            if (direction == 'right') $(this).carousel('prev');
        },
        allowPageScroll: "vertical"
    });

    $('#carousel-main, #carousel-customer ,#carousel-merchant, #carousel-main-mobile').carousel({
        interval: 5000000,
        pause: null
    });

    // ------------------ Carousel end --------------------


    // ------------------ Modal: QR Code --------------------
    $('#ozowQrCode').click(function (e) {
        e.preventDefault();
        $('#ozowQrCodeModal').modal('show')
    });

    if (window.location.href.indexOf('#ozowQrCode') != -1) {
        $('#ozowQrCodeModal').modal('show');
    }

    $('#ozowTokenisationQrCode').click(function (e) {
        e.preventDefault();
        $('#ozowTokenisationQrCodeModal').modal('show')
    });

    if (window.location.href.indexOf('#ozowTokenisationQrCode') != -1) {
        $('#ozowTokenisationQrCodeModal').modal('show');
    }


    // ------------------ Modal: QR Code end --------------------


    // ------------------ Integrations page : controls --------------------
    $(".merchant-button").addClass("active");
    $(".merchant-button").on("click", function () {
        $(".merchant").show();
        $(".status").hide();
        $(".response").hide();
        $(".merchant-button").addClass("active");
        $(".status-button").removeClass("active");
        $(".response-button").removeClass("active")
    });
    $(".response-button").on("click", function () {
        $(".response").show();
        $(".status").hide();
        $(".merchant").hide();
        $(".response-button").addClass("active");
        $(".merchant-button").removeClass("active");
        $(".status-button").removeClass("active")
    });
    $(".status-button").on("click", function () {
        $(".status").show();
        $(".response").hide();
        $(".merchant").hide();
        $(".status-button").addClass("active");
        $(".merchant-button").removeClass("active");
        $(".response-button").removeClass("active")
    });
    // ------------------ Integrations page controls end --------------------

    // ------------------ Carousel controls : Home page(mobile and desktop, Why Ozow)--------------------

    $('#carousel-main [data-slide-to="0"]').addClass('active');
    $('#carousel-main [data-text="0"]').show();
    $('#carousel-main').bind('slid.bs.carousel', function (e) {
        let currentSlide = $("#carousel-main .active").data("value");
        switch (currentSlide) {
            case 1:
                $("#carousel-main .carousel-indicators li").removeClass("active");
                $('#carousel-main [data-slide-to="0"]').addClass('active');
                $('#carousel-main [data-text="0"]').show();
                $('#carousel-main [data-text="1"]').hide();
                $('#carousel-main [data-text="2"]').hide();
                break;
            case 2:
                $("#carousel-main .carousel-indicators li").removeClass("active");
                $('#carousel-main [data-slide-to="1"]').addClass('active');
                $('#carousel-main [data-text="1"]').show();
                $('#carousel-main [data-text="2"]').hide();
                $('#carousel-main [data-text="0"]').hide();
                break;
            case 3:
                $("#carousel-main .carousel-indicators li").removeClass("active");
                $('#carousel-main [data-slide-to="2"]').addClass('active');
                $('#carousel-main [data-text="2"]').show();
                $('#carousel-main [data-text="1"]').hide();
                $('#carousel-main [data-text="0"]').hide();
                break;
        }
    });

    $('#carousel-main-mobile [data-slide-to="0"]').addClass('active');
    $('#carousel-main-mobile [data-text="0"]').show();
    $('.mobile-slider').bind('slid.bs.carousel', function (e) {
        let currentSlide = jQuery("#carousel-main-mobile .item.active").data("value");
        console.log(currentSlide);
        switch (currentSlide) {
            case 1:
                $(".mobile-slide-controls .carousel-indicators li").removeClass("active");
                $('.mobile-slide-controls [data-slide-to="0"]').addClass('active');
                $('.mobile-slide-controls [data-text="0"]').show();
                $('.mobile-slide-controls [data-text="1"]').hide();
                $('.mobile-slide-controls [data-text="2"]').hide();
                break;
            case 2:
                $(".mobile-slide-controls .carousel-indicators li").removeClass("active");
                $('.mobile-slide-controls [data-slide-to="1"]').addClass('active');
                $('.mobile-slide-controls [data-text="1"]').show();
                $('.mobile-slide-controls [data-text="2"]').hide();
                $('.mobile-slide-controls [data-text="0"]').hide();
                break;
            case 3:
                $(".mobile-slide-controls .carousel-indicators li").removeClass("active");
                $('.mobile-slide-controls [data-slide-to="2"]').addClass('active');
                $('.mobile-slide-controls [data-text="2"]').show();
                $('.mobile-slide-controls [data-text="1"]').hide();
                $('.mobile-slide-controls [data-text="0"]').hide();
                break;
        }
    });

    jQuery("#carousel-main-mobile .right").on("click", function (e) {
        e.preventDefault();
        $(".carousel").carousel('next');
    });
    jQuery("#carousel-main-mobile .left").on("click", function (e) {
        e.preventDefault();
        $(".carousel").carousel('prev');
    });


    $('#carousel-merchant [data-slide-to="0"]').addClass('active');
    $('.ozow-merchant-slider').bind('slid.bs.carousel', function (e) {
        let currentSlide = $(".ozow-merchant-slider .active").data("value");
        switch (currentSlide) {
            case 1:
                $("#carousel-merchant .carousel-indicators li").removeClass("active");
                $('#carousel-merchant [data-slide-to="0"]').addClass('active');
                $('#carousel-merchant [data-text="0"]').show();
                $('#carousel-merchant [data-text="1"]').hide();
                $('#carousel-merchant [data-text="2"]').hide();
                break;
            case 2:
                $("#carousel-merchant .carousel-indicators li").removeClass("active");
                $('#carousel-merchant [data-slide-to="1"]').addClass('active');
                $('#carousel-merchant [data-text="1"]').show();
                $('#carousel-merchant [data-text="2"]').hide();
                $('#carousel-merchant [data-text="0"]').hide();
                break;
            case 3:
                $("#carousel-merchant .carousel-indicators li").removeClass("active");
                $('#carousel-merchant [data-slide-to="2"]').addClass('active');
                $('#carousel-merchant [data-text="2"]').show();
                $('#carousel-merchant [data-text="1"]').hide();
                $('#carousel-merchant [data-text="0"]').hide();
                break;
        }
    });

    $('#carousel-customer [data-slide-to="0"]').addClass('active');
    $('.ozow-customer-slider').bind('slid.bs.carousel', function (e) {
        let currentSlide = $(".ozow-customer-slider .active").data("value");
        switch (currentSlide) {
            case 1:
                $("#carousel-customer .carousel-indicators li").removeClass("active");
                $('#carousel-customer [data-slide-to="0"]').addClass('active');
                $('#carousel-customer [data-text="0"]').show();
                $('#carousel-customer [data-text="1"]').hide();
                $('#carousel-customer [data-text="2"]').hide();
                break;
            case 2:
                $("#carousel-customer .carousel-indicators li").removeClass("active");
                $('#carousel-customer [data-slide-to="1"]').addClass('active');
                $('#carousel-customer [data-text="1"]').show();
                $('#carousel-customer [data-text="2"]').hide();
                $('#carousel-customer [data-text="0"]').hide();
                break;
            case 3:
                $("#carousel-customer .carousel-indicators li").removeClass("active");
                $('#carousel-customer [data-slide-to="2"]').addClass('active');
                $('#carousel-customer [data-text="2"]').show();
                $('#carousel-customer [data-text="1"]').hide();
                $('#carousel-customer [data-text="0"]').hide();
                break;
        }
    });

    // ------------------ Carousel controls end --------------------

});


// Blog post filter
function filterPostByCategory(categoryId, categoryName) {
    jQuery.ajax({
        method: "POST",
        url: 'https://ozow.com/wp-admin/admin-ajax.php',
        data: data = {
            'action': 'filter_posts_by_category',
            'cat_ID': categoryId,
            'cat_Name': categoryName,
        },
        beforeSend: function () {

            jQuery('.post-archive-section').html('<p style="text-align:center"><img class="img_loader" src="../~/HomePageAssets/wp-content/themes/sydney-child/images/ozowLoader.gif" /></p>');
        },
        success: function (result) {
            jQuery('.post-archive-section').html(result);
        },
        error: function (xhr, status, error) {
            // console.log(error);
        }
    });
}


// Mobile Hamburger Menu controls
function openNavR() {
    document.getElementById("mySidenavR").style.marginRight = "0px";
}

function closeNavR() {
    document.getElementById("mySidenavR").style.marginRight = "-100%";
}

