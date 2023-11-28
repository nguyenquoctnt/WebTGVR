/**
 * @license Copyright (c) 2003-2014, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.html or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    config.language = 'vi';
    // config.uiColor = '#AADC6E';

    //config.filebrowserImageBrowseUrl = '/ckfinder/ckfinder.html?type=Images';
    //config.filebrowserImageUploadUrl = '/ckfinder/core/connector/php/connector.php?command=QuickUpload&type=Images';

    //config.filebrowserBrowseUrl = '/ckfinder/ckfinder.html';

    //config.filebrowserImageBrowseUrl = '/ckfinder/ckfinder.html?type=Images';

    //config.filebrowserFlashBrowseUrl = '/ckfinder.html?type=Flash';

    // config.filebrowserUploadUrl = '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';

    //config.filebrowserImageUploadUrl = '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';

    //config.filebrowserFlashUploadUrl = '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash'; 


    config.enterMode = CKEDITOR.ENTER_BR;
    config.toolbar = 'Full';
    config.skin = 'office2013';
    config.filebrowserBrowseUrl = '/Scripts/CKFinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = '/Scripts/CKFinder/ckfinder.html?type=Images';
    config.filebrowserFlashBrowseUrl = '/Scripts/CKFinder/ckfinder.html?type=Flash';
    config.filebrowserUploadUrl = '/Scripts/CKFinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = '/Scripts/CKFinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';
    config.filebrowserFlashUploadUrl = '/Scripts/CKFinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';
    config.filebrowserWindowWidth = '1000';
    config.filebrowserWindowHeight = '700';
    config.allowedContent = true;
   
    config.extraAllowedContent = '*(*)';
    config.extraAllowedContent = '*(*);*{*}';
    config.extraAllowedContent = '*(asdf1,asdf2)';
    config.extraAllowedContent = 'p(asdf)';
    config.extraAllowedContent = '*[id]';
    config.extraAllowedContent = 'style';
    config.extraAllowedContent = 'i;span;ul;li;table;td;style;*[id];*(*);*{*}';
    config.extraAllowedContent = '*{*}';
    config.ignoreEmptyParagraph = false;
    config.extraAllowedContent = 'span(*)';
   // config.htmlEncodeOutput = true;
	
};
