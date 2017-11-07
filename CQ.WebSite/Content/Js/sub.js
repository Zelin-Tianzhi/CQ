// JavaScript Document

/*******************************
*$(function(){
* navigation(); // 导航
* DY_scroll(wraper,prev,next,img,speed,or); //游戏截图
* foldcont(btn,showdiv,defaultH); //展开收起
* zz_tab()选项卡
* dl_img(div,hoverDiv); //原画下载
* smartFloat(); //左边侧栏浮动层
* })
******************************/






//导航
function navigation(){
	$('.nav li').mousemove(function(){
		$(this).find('.s').slideDown();
	  });
	 $('.nav li').mouseleave(function(){
		$(this).find('.s').stop(true,false).slideUp("fast");
	  }); 
}



//窗口宽度
	function initwdWidth(){
    var Window_H = $(window).height();
    var Window_W = $(window).width();

    if (Window_W <= 1024) {
        $('.wrap').css({ width: '1024px' });
    }
	else{
		$('.wrap').css({ width: '100%' });
	} 
    //窗口宽度END
}
//窗口大小变化
function onwdresize(){
	
	window.onresize = function(){
//START
	//窗口宽度
	var Window_H =$(window).height();
	var Window_W =$(window).width();
	
	if(Window_W<=1024){
		$('.wrap').css({width:'1024px'});
	}
	else{
		$('.wrap').css({ width: '100%' });
	} 
	//窗口宽度END
//END
}
}

//游戏截图
var flag = "left";
function DY_scroll(wraper,prev,next,img,speed,or){  
	var wraper = $(wraper); 
	var prev = $(prev); 
	var next = $(next); 
	var img = $(img).find('ul'); 
	var w = img.find('li').outerWidth(true); 
	var s = speed; 
	next.click(function(){ 
		img.stop(true,true);
		img.animate({'margin-left':-w}/*,1500,'easeOutBounce'*/,function(){ 
			img.find('li').eq(0).appendTo(img); 
			img.css({'margin-left':0}); 
		}); 
		flag = "left";
	}); 
	prev.click(function(){ 
		img.find('li:last').prependTo(img); 
		img.css({'margin-left':-w});
		img.stop(true,true); 
		img.animate({'margin-left':0}/*,1500,'easeOutBounce'*/); 
		flag = "right";
	}); 
	if (or == true){ 
		ad = setInterval(function() { flag == "left" ? next.click() : prev.click()},s*1000); 
		wraper.hover(function(){clearInterval(ad);},function(){ad = setInterval(function() {flag == "left" ? next.click() : prev.click()},s*1000);});
	} 
} 


//展开收起
function foldcont(btn,showdiv,defaultH){
	var btn = $("."+btn);
	var div = $("."+showdiv);
	var defaultH = defaultH;//设定高度
	var areaH = div.height();//实际高度
	if(areaH < defaultH ||areaH == defaultH){
		btn.css({'visibility':'hidden'});
	}
	else{
		 div.css({'height':defaultH});
	}
	btn.click(function(){
		var areaH = div.height();
		if(areaH == defaultH){
			div.animate({'height':'100%'},200);
			$(this).css({'background':'url(images/gh_smore01.gif) no-repeat right -17px'});	
			$(this).html("收起");
		}
		else{
			div.animate({height:defaultH},200);
			$(this).css({'background':'url(images/gh_smore01.gif) no-repeat right 0'});
			$(this).html("展开");
		}
		
	});
	
}

//zz_tab()选项卡
function zz_subTab(tabHead,tabActive,tabBody){
	$('.'+tabHead).find('li').each(function(index){
		var index = index;
		var head_li = $('.'+tabHead).find('li');
		var body_li = $('.'+tabBody);
		body_li.hide();
		body_li.eq(0).show();
		$(this).click(function(){
			head_li.removeClass(tabActive);
			head_li.eq(index).addClass(tabActive);
			body_li.hide();
			body_li.eq(index).show();
		})
	});
}


//原画下载
function dl_img(div,hoverDiv){
	var img_li = $('.'+div);
	img_li.each(function(){
		var picHover = $(this).find('.'+hoverDiv);
		$(this).hover(function(){
			picHover.show();
		},function(){
			picHover.hide();
		});
	});
}


//左边侧栏浮动层
$.fn.smartFloat = function() {
	var position = function(element) {
		var top = element.position().top, pos = element.css("position");
		var flTop = 674;
		$(window).scroll(function() {
			var heightR = $(".sider_right").height() - 552;
			var scrolls = $(this).scrollTop();
			if (scrolls > top+flTop) {
				if(window.XMLHttpRequest){//非ie6
					if(scrolls > heightR){
						element.css({
							position: pos,
							top: heightR-flTop	
						}); 
					}else{
						element.css({
							position:"fixed",	
							top: -568
						});
					}
				}
				else{//ie6
					if(scrolls > heightR){ scrolls = heightR;}
					element.css({
						top: scrolls-flTop
					});
				}	
			}else {
				element.css({
					position: pos,
					top: top
				});	
			}
		});
};
	return $(this).each(function() {
		position($(this));						 
	});
};
$(function(){
	$(".sub_left").smartFloat();//左边侧栏浮动层
})

//弹窗
function showDiv(alink){
	var alink = alink;//跳转的网址
	var popDiv = $("#popDiv");//弹出层
	var popValue = getCookie("popKey");
	if (popValue!=null && popValue!=""){
		window.open(alink);
	}
	else{
		popDiv.show();
		popBtn(popDiv,alink);
		
	}
	
}
function popBtn(popDiv,alink){
	$(".pop_confirm").click(function(){
		popDiv.hide();
		setCookie("popKey","yes",7);
		
		window.open(alink);
		
	});
	$(".pop_cancel").click(function(){
		popDiv.hide();
	});
	$(".pop_close").click(function(){
		popDiv.hide();
	});
}

function setCookie(c_name,value,expiredays)
{
var exdate=new Date()
exdate.setDate(exdate.getDate()+expiredays)
document.cookie=c_name+ "=" +escape(value)+
((expiredays==null) ? "" : ";expires="+exdate.toGMTString())
}

function getCookie(c_name)
{
if (document.cookie.length>0)
  {
  c_start=document.cookie.indexOf(c_name + "=")
  if (c_start!=-1)
    { 
    c_start=c_start + c_name.length+1 
    c_end=document.cookie.indexOf(";",c_start)
    if (c_end==-1) c_end=document.cookie.length
    return unescape(document.cookie.substring(c_start,c_end))
    } 
  }
return ""
}

