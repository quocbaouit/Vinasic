$(document).ready(function () {
    $('[btn="close-left"]').click(function () {
        if ($(window).width() >= 1185)
        {            
            if ($('#content-left').css('left') != '2px')
            {
                $('#content-right').animate({ width: "82.7%" }, 700);
                $('#content-left').animate({ left: "2px" }, 700);
                // call delay 1000 then remove class
                setTimeout(function () {
                    $('#content-left').removeClass('content-left-position');                    
                }, 700);               
                $('#button-menu-left').removeClass('button-menu-left-hide');
                $('#MenuLeftAcc').css('display', 'block !important');
                // set the tooltip content
                $('#button-menu-left').attr('title', 'Ẩn Menu Trái.');                
            }
            else
            {
                $('#content-left').animate({ left: '-230px' }, 700);
                $('#content-right').animate({ width: "100%" }, 700);
                $('#content-left').addClass('content-left-position');        
                $('#button-menu-left').addClass('button-menu-left-hide');
                // set the tooltip content
                $('#button-menu-left').attr('title', 'Hiện Menu Trái.');
            }
        }
        else {

            if ($('#content-left').css('left') == '2px')
            {
                $('#content-left').animate({ left: '-230px' }, 700);
                // call delay 1000 then remove class
                setTimeout(function () {
                    $('#content-left').removeClass('content-left-position');
                }, 700);
                $('#button-menu-left').removeClass('button-menu-left-hide-small');
                $('#MenuLeftAcc').removeClass('menuLeftAcc-Block');
                // set the tooltip content
                $('#button-menu-left').attr('title', 'Hiện Menu Trái.');
                //alert('none');
            }
            else
            {
                $('#content-left').animate({ left: '2px' }, 700); 
                $('#button-menu-left').addClass('button-menu-left-hide-small'); 
                $('#MenuLeftAcc').addClass('menuLeftAcc-Block');
                // set the tooltip content
                $('#button-menu-left').attr('title', 'Ẩn Menu Trái.');

            }
        }
    });

    $(window).resize(function () {
        if ($(window).width() >= 1185) {
            $('#content-right').css('width', '82.7%');
            $('#content-left').css('left', '2px');
            $('#button-menu-left').removeClass('button-menu-left-hide');
            $('#button-menu-left').removeClass('button-menu-left-hide-small');
            $('#button-menu-left').attr('title', 'Ẩn Menu Trái.');
        }
        else {
            $('#content-right').css('width', '100%');
            $('#content-left').css('left', '-230px');
            $('#content-left').addClass('content-left-position');
            $('#button-menu-left').addClass('button-menu-left-hide');
            // set the tooltip content
            $('#button-menu-left').attr('title', 'Hiện Menu Trái.');
        }
    });
});
