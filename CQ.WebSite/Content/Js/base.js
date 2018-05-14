// JavaScript Document
/*******************************
* 
******************************/
$(function() {
	initwdWidth(); //窗口大小变化
	onwdresize(); //窗口大小变化
})
$.request = function (name) {
    var search = location.search.slice(1);
    var arr = search.split("&");
    for (var i = 0; i < arr.length; i++) {
        var ar = arr[i].split("=");
        if (ar[0] == name) {
            if (unescape(ar[1]) == 'undefined') {
                return "";
            } else {
                return unescape(ar[1]);
            }
        }
    }
    return "";
}
window.prop = 1;//定义充值比例

//充值
function ShowPayType(name){
	switch(name){
		case "ZHIFUBAO"://
			var dom = '<li class="ZHIFUBAO select"><span>支付宝</span></li>';
			break;
		case "WEIXIN"://
			var dom = '<li class="WEIXIN select"><span>微信</span></li>';
			break;
		case "WANGYIN"://
			var dom = '<li class="ICBC select"><span>工商银行</span></li>\
                        <li class="BOC"><span>中国银行</span></li>\
                        <li class="ABC"><span>农业银行</span></li>\
                        <li class="CCB"><span>建设银行</span></li>\
                        <li class="PSBC"><span>邮政银行</span></li>\
                        <li class="BCOM"><span>交通银行</span></li>\
                        <li class="CMB"><span>招商银行</span></li>\
                        <li class="CIB"><span>兴业银行</span></li>\
                        <li class="CMBC"><span>民生银行</span></li>\
                        <li class="CITIC"><span>中信银行</span></li>\
                        <li class="CEB"><span>光大银行</span></li>\
                        <li class="PAB"><span>平安银行</span></li>\
                        <li class="HXB"><span>华夏银行</span></li>\
                        <li class="GDB"><span>广发银行</span></li>\
                        <li class="SDB"><span>深发银行</span></li>\
                        <li class="SPDB"><span>浦发银行</span></li>\
                        <li class="SHB"><span>上海银行</span></li>';
			break;
		case "DIANKA"://
			var dom = '<li class="JUNKA select"><span>骏网卡</span></li>\
                        <li class="SHENDA"><span>盛大卡</span></li>\
                        <li class="WANGYI"><span>网易卡</span></li>\
                        <li class="WANMEI"><span>完美卡</span></li>\
                        <li class="JIUYOU"><span>久游卡</span></li>\
                        <li class="SHOUHU"><span>搜狐卡</span></li>\
                        <li class="ZHENTU"><span>征途卡</span></li>\
                        <li class="QQ"><span>Q币卡</span></li>';
			break;
		case "SHOJICHONGZHIKA"://
			var dom = '<li class="MOBILE select"><span>中国移动</span></li>\
	                    <li class="UNICOM"><span>中国联通</span></li>\
	                    <li class="TELECOM"><span>中国电信</span></li>';
			break;
	}
	$(".recharge_list li").on("click",function(){
		$(".recharge_list li").removeClass('current');
		$(this).addClass('current');
		return false;
	})
	$('.bankSelect').html('');
	$('.bankSelect').append(dom);
	$(".bankSelect li").on("click",function(){
		$(".bankSelect li").removeClass('select');
		$(this).addClass('select');
		return false;
	})
	ShowPaydata();
}	

function ShowPayVal(Payval){
	$("#amountList_child li").on("click",function(){
		$("#amountList_child li span").removeClass('select');
		$(this).children('span').addClass('select');
		return false;
	})
	$('#pay_center_submit_amount').html('￥'+Payval);
	$('#pay_center_submit_getjb').html(Payval*prop);
}

function onloadObj(){
	$("#amountList_child li").on("click",function(){
		$("#amountList_child li span").removeClass('select');
		$(this).children('span').addClass('select');
		return false;
	})
	$(".bankSelect li").on("click",function(){
		$(".bankSelect li").removeClass('select');
		$(this).addClass('select');
		return false;
	})
	
}



//窗口宽度
function initwdWidth(){
    var Window_H = $(window).height();
    var Window_W = $(window).width();
    if (Window_W <= 1374) {
    	if(Window_W<1000){
    		var wrapW =1000;
    		$('.container').width(wrapW);
    	}else{
    		var wrapW =Window_W-20;
    	}
    	var bannerW = wrapW-376-10;
    	var bigimgW = wrapW-376-10-269;
    	var imgML = -(710-bigimgW)/2;
    	$('.active-banner').width(bannerW);
    	$('.ad-big-img').width(bigimgW);
    	$('.ad-big-img img').css({'margin-left':imgML+'px'});
    	
        $('.wrap').width(wrapW);
        $('.girl').css({top:'10px'});
		$('.girl1').css({left:'0'});
		$('.girl2').css({right:'0'});
    }
	else{
		if (Window_W < 1864){
			$('.girl').css({top:'10px'});
			$('.girl1').css({left:'140px'});
			$('.girl2').css({right:'136px'});
		}else {
			$('.girl').css({top:'216px'});
			$('.girl1').css({left:'-17px'});
			$('.girl2').css({right:'0'});
		}
		$('.wrap').width(1374);
		$('.active-banner').width(980);
    	$('.ad-big-img').width(710);
		$('.ad-big-img img').css({'margin-left':'0'});
	} 
}
function onwdresize(){
	window.onresize = function(){
		initwdWidth();
	}
}

