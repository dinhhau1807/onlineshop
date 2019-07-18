﻿var product = {
    init: function () {
        product.registerEvent();
    },
    registerEvent: function () {
        $('.btn-images').off('click').on('click', function (e) {
            e.preventDefault();
            $('#imagesManage').modal('show');
            $('#hidProductID').val($(this).data('id'));
            product.loadImages();
        });

        $('#btnChoseImages').off('click').on('click', function (e) {
            e.preventDefault();
            var finder = new CKFinder();
            finder.selectActionFunction = function (url) {
                $('#imageList').append(`<div style="float:left"><img src="${url}" width="100" /><a href="#" class="btn-delImage"><i class="fas fa-times"></i></a></div>`);
                $('.btn-delImage').off('click').on('click', function (e) {
                    e.preventDefault();
                    $(this).parent().remove();
                })
            };
            finder.popup();
        });

        $('#btnSaveImages').off('click').on('click', function () {
            var images = [];
            var id = $('#hidProductID').val();

            $.each($('#imageList img'), function (i, item) {
                images.push($(item).prop('src'));
            });

            $.ajax({
                url: '/Admin/Product/SaveImages',
                type: 'POST',
                data: {
                    id: id,
                    images: JSON.stringify(images)
                },
                dataType: 'json',
                success: function (response) {
                    if (response.status == true) {
                        $('#imagesManage').modal('hide');
                        $('#imageList').html('');
                        alert('Lưu thành công!');
                    }
                }
            });
        });
    },
    loadImages: function () {
        $.ajax({
            url: '/Admin/Product/LoadImages',
            type: 'GET',
            data: {
                id: $('#hidProductID').val()
            },
            dataType: 'json',
            success: function (response) {
                var data = response.data;
                var html = '';
                $.each(data, function (i, item) {
                    html += `<div style="float:left"><img src="${item}" width="100" /><a href="#" class="btn-delImage"><i class="fas fa-times"></i></a></div>`;
                });
                $('#imageList').html(html);

                $('.btn-delImage').off('click').on('click', function (e) {
                    e.preventDefault();
                    $(this).parent().remove();
                })
            }
        });
    }
};

product.init();