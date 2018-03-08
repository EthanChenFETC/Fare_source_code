/** © 2013 新逸科技 2014-04-19 by Vincent **/
(function( window ) {

    if (typeof garden == 'undefined') {
        window.garden = {};
    }

    garden.util = {

        /**
         * 轉換XML跳脫字元
         * @param text
         * @return {String}
         */
        escapeXml: function escapeXml(text) {
            var ret = text + '';
            ret = ret.replace(/&/g, '&amp;');
            ret = ret.replace(/</g, '&lt;');
            ret = ret.replace(/>/g, '&gt;');
            return ret;
        },
        /**
         * 跳脫XML->HTML跳脫字元
         * @param text
         * @return {String}
         */
        escapeXmlHtml: function escapeXmlHtml(text) {
            var ret = text + '';
            ret = garden.util.escapeXml(ret);
            ret = ret.replace(/&/g, '%26');
            ret = ret.replace(/;/g, '%3B');
            return ret;
        },
        /**
         * 還原XML跳脫字元
         * @param text
         * @return {String}
         */
        unEscapeXml: function(text) {
            var ret = text + '';
            ret = ret.replace(/&amp;/g, '&');
            ret = ret.replace(/&lt;/g, '<');
            ret = ret.replace(/&gt;/g, '>');
            return ret;
        },
        /**
         * 還原XML->HTML跳脫字元
         * @param text
         * @return {String}
         */
        unEscapeXmlHtml: function(text) {
            var ret = text + '';
            ret = ret.replace(/%26/g, '&');
            ret = ret.replace(/%3B/g, ';');
            ret = garden.util.unEscapeXml(ret);
            return ret;
        },
        /**
         *
         * @param text
         */
        toOneLineString : function(text) {
            var ret = text + '';
            ret = ret.replace(/\r\n/g, '');
            ret = ret.replace(/\n/g, '');
            ret = ret.replace(/\t/g, '');
            return ret;
        },
        /**
         * 轉換DOM成String
         * @param dom
         * @return {String}
         */
        parseDOM2String: function parseDOM2String(dom) {
            dom = $(dom);
            var text;
            if (window.XMLSerializer) {
                text = new XMLSerializer().serializeToString(dom[0]);
            }
            //Internet Explorer
            else {
                text = dom[0].xml;
            }
            return text;
        },
        /**
         * 轉換String成DOM
         * @param text
         * @return {jQuery|HTMLElement}
         */
        parseString2DOM: function parseString2DOM(text) {
            console.log('parseString2DOM ' + (typeof text));
            var xmlDoc = (typeof text == 'object' ? text : $(garden.util._parseString2DOM(text)));
            return xmlDoc;
        },
        /**
         *
         * @param text
         * @return {jQuery|HTMLElement}
         * @private
         */
        _parseString2DOM: function _parseString2DOM(text) {
            console.log('_parseString2DOM ' + (typeof text));
            if ((!text || text == '') || text.indexOf('<') == -1) {
                text = '<parserError>parserError</parserError>';
            }
            var xmlDoc;
            if (window.DOMParser) {
                xmlDoc = new DOMParser().parseFromString(text, 'text/xml');
            }
            //Internet Explorer
            else {
                xmlDoc = new ActiveXObject('Microsoft.XMLDOM');
                xmlDoc.async = 'false';
                xmlDoc.loadXML(text);
            }
            try {
                if ($.isXMLDoc(xmlDoc)) {
                    if ($('parsererror', xmlDoc).length != 0) {
                        xmlDoc = garden.util._parseString2DOM('<parserError>parserError</parserError>');
                    }
                }
            } catch(e) {
                xmlDoc = garden.util._parseString2DOM('<parserError>parserError</parserError>');
            }

            return xmlDoc;
        },
        /**
         *
         * @param parameterName
         * @return {Array}
         */
        getParameter: function getParameter(parameterName) {
            var queryString = window.top.location.search.substring(1);

            var values = new Array();

            if (queryString.length > 0 && parameterName) {
                var params = queryString.split('&');
                for (var i = 0; i < params.length; i++) {
                    var param = params[i].split('=');
                    if (param[0] == parameterName) {
                        values.push(param[1]);
                    }
                }
            }
            return values;
        },
        getParameterNames: function getParameterNames() {
            var queryString = window.top.location.search.substring(1);

            var parameterNames = [];

            if (queryString.length > 0) {
                var params = queryString.split('&');
                for (var i = 0; i < params.length; i++) {
                    var param = params[i].split('=');
                    if ($.inArray(param[0], parameterNames) == -1) {
                        parameterNames.push(param[0]);
                    }
                }
            }
            return parameterNames;
        },

        getApURL: function() {
            return _garden._ap.protocol + '//' + _garden._ap.host + ( _garden._ap.port ? ':' + _garden._ap.port : '')
                + '/' + _garden._ap.path + '/';
        },

        getFullApURL: function() {
            return _garden._ap.protocol + '//' + _garden._ap.host + ( _garden._ap.port ? ':' + _garden._ap.port : '')
               + _garden._ap.pathname;
        },

        getApId: function() {
            return _garden._ap.host + '_' + _garden._ap.path;
        },

        disableCookie: function(cookieName) {
            $.cookie(cookieName, '', {
                expires: -1
            });
        },

        checkPermit: function(response) {
            try {
                if (response.record !== undefined){
                    if (response.record.exception !== undefined) {
                        $('.garden-message').remove();
                        garden.util.ui.error(response.record.exception);
                        garden.util.ui.loading('hide');
                        return false;
                    }
                } else {
                    if (response.find('exception').length != 0) {
                        $('.garden-message').remove();
                        garden.util.ui.error(response.find('exception').text());
                        garden.util.ui.loading('hide');
                        return false;
                    }
                }
            } catch(e) {
                //                console.warn(e);
            }

            try {
                if (response.record !== undefined) {
                    if (response.record.noSession !== undefined) {
                        garden.logout(response.record.noSession);
                        return false;
                    }
                } else {
                    if (response.find('noSession').length != 0) {
                        garden.logout(response.find('noSession').text());
                        return false;
                    }
                }
            } catch(e) {
                //                console.warn(e);
            }

            try {
                if (response.record !== undefined) {
                    if (response.record.noPermit !== undefined) {
                        alert(response.record.noPermit);
                        garden.logout();
                        return false;
                    }
                } else {
                    if (response.find('noPermit').length != 0) {
                        alert(response.find('noPermit').text());
                        garden.logout();
                        return false;
                    }
                }
            } catch(e) {
                //                console.warn(e);
            }
            return true;
        },

        loadJSResource: function(url, async) {
            $.each($.makeArray(url), function(i, url){
                var _URL = garden.util.getApURL() + url;
                $.ajax({
                    url: _URL,
                    cache: true,
                    dataType: 'script',
                    async: (async ? async : false),
                    success: function() {
                        console.log('load js file ' + _URL + ' success.');
                    },
                    error: function(){
                        console.error('load js file ' + _URL) + ' fail!';
                    }
                });
            });
        },

        loadCSSResource: function(url) {
            $.each($.makeArray(url), function(i, url) {
                $('link:last').after('<link type="text/css" rel="stylesheet" href="' + url + '">');
            });
        },

            _initCSS: function(type) {
            // 初始化處理css
            var $_bootstrapCSS = $('link[data-type="bootstrap"]'),
                $_bootstrapResponsiveCSS = $('link[data-type=bootstrap-responsive]'),
                $_gardenCSS, $_gardenResponsiveCSS
            ;
            $_gardenCSS = $('<link rel="stylesheet" href="garden/' + type + '/css/garden.css" data-type="garden">');
            $_gardenResponsiveCSS = $('<link rel="stylesheet" href="garden/' + type + '/css/garden-responsive.css" data-type="garden-responsive.css">');

            if ($_bootstrapCSS.length > 0) {
                $_gardenCSS.insertAfter($_bootstrapCSS);
            } else {
                $_gardenCSS.insertAfter($('head title'));
            }

            if ($_bootstrapResponsiveCSS.length > 0) {
                $_gardenResponsiveCSS.insertAfter($_bootstrapResponsiveCSS);
            } else {
                $_gardenResponsiveCSS.insertAfter($_gardenCSS);
            }

            if (type == 'admin') {
                $_bootstrapResponsiveCSS.remove();
            }
        },

        getSysLanguageThesaurus: function(lang) {
            $.ajax({
                async: false,
                url: 'garden/_system/i18n/' + lang,
                dataType: 'json',
                success: function(resp, status, jqXHR) {
                    if (resp) {
                        var thesaurus = {};
                        $.each($.makeArray(resp.record.i18n), function(i, item){
                            thesaurus[item.id] = item.content;
                        });
                        _garden._system.i18n.setThesaurus(thesaurus);
                    }
                    else {
                        console.warn('i18n system pack is empty!');
                    }

                    // 設定到系統資訊裡
                    garden.browser.language = lang;
                },
                error: function(jqXHR, status, errorThrown) {
                    console.warn('i18n system pack connect error! [status:' + status + ',error:' + errorThrown + ']');
                }
            });
        },

        type: {
            isFunction: function isFunction(arg) {
                return $.isFunction(arg);
            },
            isArray: function isArray(arg) {
                return $.isArray(arg);
            },
            isObject: function isObject(arg) {
                return (typeof arg === 'object' && !garden.util.type.isArray(arg))
            }
        },

        ui: {
            overlay: function(settings) {

                // 預設
                var _default = {
                        close: false,
                        lock: false
                    },
                    _settings = $.extend(_default, settings),
                    $overlay = $('div.garden-overlay:not(.garden-remove)'),
                    $window = $(window),
                    $document = $(document);

                // 取得或建立模板
                if ($overlay.length == 0) {

                    $overlay = $('<div class="garden-overlay"></div>').appendTo($(document.body));

                    // 是否有綁定關閉事件
                    if (_settings.close && garden.util.type.isFunction(_settings.close)) {
                        setTimeout(function() {
                            $overlay.bind('click', _settings.close);
                        }, 100);
                    } else {
                        setTimeout(function() {
                            $overlay.bind('click', function(event){
                                garden.util.ui.loading('hide');
                                garden.util.ui.closeDialog($('div.garden-dialog:visible'));
                            });
                        }, 100);
                    }
                    // 調整成滿版
//                    $overlay.css({
//                        width: $document.width(),
//                        height: $document.height()
//                    });

                    if (_settings.lock) {
                        var $scrollElem = $.browser.safari ? $('body') : $('html'),
                            _scrollTop = $scrollElem.scrollTop(),
                            _scrollLeft = $scrollElem.scrollLeft()
                        ;
                        $window.bind('scroll',function () {
                            $window.scrollTop(_scrollTop).scrollLeft(_scrollLeft);
                        });
                    }
                }

                return $overlay;
            },

            closeOverlay: function(callback) {

                var ret = true,
                    $overlay;

                if (garden.util.type.isFunction(callback)) {
                    ret = callback();
                }

                if (ret) {
                    $overlay = $('div.garden-overlay').addClass('garden-remove');
                    $overlay.slideUp(function() {
                        $overlay.remove();
                        $(window).unbind('scroll');
                    });
                }

                return ret;
            },

            loading: function loading(type, word) {
                var $load;
                if (type === 'show') {
                    $('body').append('<div class="garden-loading-overlay"></div>');
                    $load = garden.util.ui.openLoading(word);
                }
                else if (type === 'hide') {
                    garden.util.ui.closeLoading();
                    $('body').find('.garden-loading-overlay').remove();
                }
                else {
                    console.warn('undefined loading type:' + type);
                }
                return $load;
            },
            openLoading: function garden_openLoading(word) {

                var $load;

                if (!word) {
                    word = 'loading';
                }

                $load = $('<div class="garden-loading-message">' +
                    '<img class="icon" src="garden/img/ajax-loader.gif" />' +
                    '<span class="text">' + word + '</span></div>').appendTo($('body'));

                $load.css({
                    'top': (($(window).height() - $load.height()) / 2) + $(document).scrollTop(),
                    'left': (($(window).width() - $load.width()) / 2) + $(document).scrollLeft()
                });

                return $load;
            },
            closeLoading: function garden_closeLoaing() {
                $('body').find('.garden-loading-message').remove();
                return true;
            },

            dialog: function garden_dialog(fn, args) {

                var $overlay,
                    $dialog,
                    $header,
                    $close;

                // 隱藏目前的dialog
                $('div.garden-dialog').hide();
                //
                $dialog = $('<div class="garden-dialog"></div>').appendTo($(document.body));
                //
                $header = $('<div class="dialog-header"></div>').appendTo($dialog);
                // 設定關閉按鈕
//                $('<div class="garden-close" title="' + garden.i18n.convert('close') + '" />').prependTo($dialog).bind('click', function() {
                $('<button aria-hidden="true" class="close" type="button">×</button>').prependTo($header).bind('click', function() {
                    garden.util.ui.closeDialog($dialog);
                });
                // 執行傳入的
                if (fn) {
                    fn($dialog, args);
                    // 設定大小與顯示位置
                    garden.util.ui.resizeDialog($dialog);
                }

                $overlay = garden.util.ui.overlay();

                return $dialog;
            },

            resizeDialog: function garden_resizeDialog(el, w_zoom, h_zoom, isFixed) {

                var $window = $(window),
                    _sw = el.width(),
                    _sh = el.height(),
                    _sow = el.outerWidth(),
                    _soh = el.outerHeight(),
                    _ww = $window.width(),
                    _wh = $window.height(),
                    _zw, _zh, _zow, _zoh,
                    _top, _left;
                // 取得container內外差(margin+border+padding+content)
                _zow = _sow - _sw;
                _zoh = _soh - _sh;
                // 取得縮放比例
                w_zoom = w_zoom || (_ww >= 1024 ? 0.85 : 0.98);
                h_zoom = h_zoom || (_wh >= 768 ? 0.85 : 0.9);
                // window縮放比例後的大小
                _zw = _ww * w_zoom;
                _zh = _wh * h_zoom;
                // 判斷用本體大小還是要用縮放大小
                _sw = (_sw + _zow > _zw || isFixed ? _zw - _zow : _sw);
                _sh = (_sh + _zoh > _zh || isFixed ? _zh - _zoh : _sh);
                // 定位的左方與上方位置
                _left = ((_ww - _sw  - _zow) / 2);
                _top = (_sh + _zoh > _zh || isFixed ? ((_wh - _sh  - _zoh) / 2) : ((_wh - _sh  - _zoh) / 4));
                // 處理css
                el.css({
                    position: 'fixed',
                    left : _left,
                    top: _top,
                    width: _sw,
                    height: _sh
                });

                return true;
            },

            closeDialog: function garden_closeDialog(el) {
                if (el) {
                    el.remove();
                } else {
                    $('div.garden-dialog').last().remove();
                }
                garden.util.ui.closeOverlay(function(){
                    var $dialogs = $('div.garden-dialog');
//                    console.log('closeOverlay-closeDialog:' + $dialogs.length);
                    // 如果只剩一個dialog救回傳true去關閉overlay
                    var isClose = ($dialogs.length == 0);
                    // 如果還有兩個,要把最後的前一個show出來,再移除最後一個
                    if (!isClose) {
                        var $last = $dialogs.last();
                        $last.show();
                        $last.children('div').show();
                    }
                    return isClose;
                });
            },

            message: function garden_message(message, view) {
                var _device = window._garden.device;
                switch(_device) {
                    case 'pc':
                        var $view,
                            $message = $('<div class="garden-message" />').append(message).appendTo($('body')),
                            top;
                        top = view ? ($('#' + view.id).find('form').offset().top - 10) : '30%';
                        $message.css('top', top);
                        setTimeout(function() {
                            $message.fadeOut(function() {
                                $message.remove();
                            });
                        }, 2000);
                        break;
                    case 'mobile':
                        garden.util.ui.alert(message);
                        break;
                    default:
                        console.error('unknown device[' + _device + ']');
                        break;
                }
            },

            alert: function garden_alert() {

                var $alert;

                var title = '',message, isNeedOverlay = true,
                    callback,
                    options = {
                        type: 'info'
                    };

                for (var i = 0; i < arguments.length; i++) {
                    var argument = arguments[i];
                    var argType = $.type(argument);
                    if (argType === 'string') {
                        if (message) {
                            title = message;
                            message = argument;
                        } else {
                            message = argument;
                        }
                    } else if (argType === 'boolean') {
                        isNeedOverlay = argument;
                    } else if (argType === 'function') {
                        callback = argument;
                    } else if (argType === 'object') {
                        options = $.extend(options, argument);
                    }
                }

                var _device = window._garden.device;
                switch(_device) {
                    case 'pc':
                        var $dialog = garden.util.ui.dialog();
                        $dialog.addClass('garden-alert garden-' + options.type);
                        $dialog.append('<div class="garden-icon" />')
                            .append('<div class="garden-dialog-msgPanel"><h2>' + title + '</h2><div class="garden-msg">' + message.replace(/\n/g, '<br />') + '</div></div>');
                        $(document).bind('keydown', function(event) {
                            if (event.which == 13 || event.which == 32) {
                                $dialog.find('.garden-close').trigger('click');
                                $(this).unbind('keydown', arguments.callee);
                            }
                        });
                        garden.util.ui.resizeDialog($dialog);
                        $alert =  $dialog;

                        break;
                    case 'mobile':
                        var $header = $('header'),
                            $alert;
                        var uuid = Math.uuid(),
                            id = 'alt_' + uuid,
                            html = '<div id="' + id + '" class="ge-m alert alert-' + options.type + '">' +
                            '<button type="button" class="close" data-dismiss="alert">×</button>' +
                            '<strong>' + title + '</strong>' +
                            ' ' + message +
                            '</div>';
                        $alert = $(html).appendTo($('body'));
                        $alert.hide();

                        // 500秒後呈現
                        setTimeout(function() {
                            $alert.slideDown('fast');
                        }, 500);

                        setTimeout(function() {
                            $('#' + id).remove();
                        }, 5000);

                        break;
                    default:
                        console.error('unknown device[' + _device + ']');
                        break;
                };

                return $alert;
            },

            error: function garden_error() {

                var options = {
                    type: 'error'
                }, args = [];

                for (var i = 0; i < arguments.length; i++) {
                    args.push(arguments[i]);
                }
                args.push(options);

                var $alert = garden.util.ui.alert.apply(this, args);

                return $alert;

//                var title = '';
//                var message;
//                var callback;
//                var isNeedOverlay = true;
//                for (var i = 0; i < arguments.length; i++) {
//                    var argument = arguments[i];
//                    var argType = $.type(argument);
//                    if (argType === 'string') {
//                        if (message) {
//                            title = message;
//                            message = argument;
//                        } else {
//                            message = argument;
//                        }
//                    } else if (argType === 'boolean') {
//                        isNeedOverlay = argument;
//                    } else if (argType === 'function') {
//                        callback = argument;
//                    }
//                }
//
//                var $dialog = garden.util.ui.dialog();
//
//                $dialog.addClass('garden-error');
//
//                $dialog.append('<div class="garden-icon" />')
//                    .append('<div class="garden-dialog-msgPanel"><h2>' + title + '</h2><div class="garden-msg">' + message.replace(/\n/g, '<br />') + '</div></div>');
//
//                $(document).bind('keydown', function(event) {
//                    if (event.which == 13 || event.which == 32) {
//                        $dialog.find('garden-close').trigger('click');
//                        $(this).unbind('keydown', arguments.callee);
//                    }
//                });
//
//                garden.util.ui.resizeDialog($dialog);
            },
            confirm: function(message, title, agreeFn, cancelFn) {
                var $dialog = garden.util.ui.dialog();
                $dialog.addClass('garden-confirm');

                title = title || '';
                message = message || '';

                $dialog.append('<div class="garden-icon" />')
                    .append('<div class="garden-dialog-msgPanel"><h2>' + title + '</h2><div class="garden-msg">' + message.replace(/\n/g, '<br />') + '</div></div>');

                var $panelDiv = $dialog.find('div.garden-dialog-msgPanel');
                var $btnDiv = $('<div class="garden-buttons"></div>').appendTo($panelDiv);
                $('<button type="button" class="btn btn-primary">agree</button>')
                    .appendTo($btnDiv).bind('click', function(event) {
                        garden.util.ui.closeDialog($dialog);
                        agreeFn();
                    });

                $('<button type="button" class="btn">cancel</button>')
                    .appendTo($btnDiv).bind('click', function(event) {
                        garden.util.ui.closeDialog($dialog);
                        cancelFn();
                    });

                garden.util.ui.resizeDialog($dialog);
            }
        },

        validate: {
            //驗證email
            validateEmail: function(email) {
                //            return !/[\uFE30-\uFFA0]/i.test(email) && /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i.test(email);
                return !/[\uFE30-\uFFA0]/i.test(email) && /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/.test(email);
            },

            //驗證身分證字號
            validateIdentifier: function(identifier) {
                var ret = true;

                identifier = identifier + '';
                if (identifier.length != 10) {
                    ret = false;
                } else {
                    var localeCharacter = identifier.substr(0, 1);
                    var sexCharacter = identifier.substr(1, 1);
                    var validateCharacter = identifier.substr(2, 9);

                    if (!/[A-Z]/.test(localeCharacter)) {
                        ret = false;
                    } else if (!/[1-2]/.test(sexCharacter)) {
                        ret = false;
                    } else if (isNaN(validateCharacter)) {
                        ret = false;
                    } else {
                        identifier = garden.util.validate._identifierLocaleCode(identifier);
                        var c1 = parseInt(identifier.substr(0, 1) , 10);
                        var c2 = parseInt(identifier.substr(1, 1) , 10);
                        var c3 = parseInt(identifier.substr(2, 1) , 10);
                        var c4 = parseInt(identifier.substr(3, 1) , 10);
                        var c5 = parseInt(identifier.substr(4, 1) , 10);
                        var c6 = parseInt(identifier.substr(5, 1) , 10);
                        var c7 = parseInt(identifier.substr(6, 1) , 10);
                        var c8 = parseInt(identifier.substr(7, 1) , 10);
                        var c9 = parseInt(identifier.substr(8, 1) , 10);
                        var c10 = parseInt(identifier.substr(9, 1) , 10);
                        var check = parseInt(identifier.substr(10, 1) , 10);

                        var total = c1 * 1 + c2 * 9 + c3 * 8 + c4 * 7 + c5 * 6 + c6 * 5 + c7 * 4 + c8 * 3 + c9 * 2 + c10 * 1;

                        ret = (check == (10 - (total % 10))) || (check == 0 && total % 10 == 0);
                    }
                }
                return ret;
            },

            _identifierLocaleCode: function(identifier) {
                var localeCharacter = identifier.substr(0, 1);
                var localeMapping = {
                    'A': '10',
                    'B': '11',
                    'C': '12',
                    'D': '13',
                    'E': '14',
                    'F': '15',
                    'G': '16',
                    'H': '17',
                    'I': '34',
                    'J': '18',
                    'K': '19',
                    'L': '20',
                    'M': '21',
                    'N': '22',
                    'O': '35',
                    'P': '23',
                    'Q': '24',
                    'R': '25',
                    'S': '26',
                    'T': '27',
                    'U': '28',
                    'V': '29',
                    'W': '32',
                    'X': '30',
                    'Y': '31',
                    'Z': '33'
                };

                var mapping = localeMapping[localeCharacter];

                identifier = mapping + identifier.substr(1 , 9);

                return identifier;
            }
        }
    };

  

})( window );