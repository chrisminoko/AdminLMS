jQuery(document).ready(function ($) {
    // Blog slider
    $('#recentPostSlider').slick({
        slidesToScroll: 2,
        variableWidth: true,
        responsive: [
            {
                breakpoint: 500,
                settings: {
                    arrows: true,
                    centerMode: true,
                    slidesToScroll: 1,
                    slidesToShow: 1
                }
            }],
        prevArrow: "<button type='button' class='slick-prev pull-left'><i class='la la-angle-left' aria-hidden='true'></i></button>",
        nextArrow: "<button type='button' class='slick-next pull-right'><i class='la la-angle-right' aria-hidden='true'></i></button>"
    });
    $('#pressReleasePostSlider').slick({
        variableWidth: true,
        slidesToScroll: 2,
        responsive: [
            {
                breakpoint: 500,
                settings: {
                    arrows: true,
                    centerMode: true,
                    slidesToScroll: 1,
                    slidesToShow: 1
                }
            }],
        prevArrow: "<button type='button' class='slick-prev pull-left'><i class='la la-angle-left' aria-hidden='true'></i></button>",
        nextArrow: "<button type='button' class='slick-next pull-right'><i class='la la-angle-right' aria-hidden='true'></i></button>"
    });

    //Single blog slider
    $('#recentPostSliderSingle').slick({
        variableWidth: true,
        slidesToScroll: 2,
        prevArrow: "<button type='button' class='slick-prev pull-left'><i class='la la-angle-left' aria-hidden='true'></i></button>",
        nextArrow: "<button type='button' class='slick-next pull-right'><i class='la la-angle-right' aria-hidden='true'></i></button>",
        responsive: [
            {
                breakpoint: 500,
                settings: {
                    arrows: true,
                    centerMode: true,
                    slidesToScroll: 1,
                    slidesToShow: 1
                }
            }],
    });
});