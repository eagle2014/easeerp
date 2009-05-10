/*********************************************************************** 
* File    : JSFX_ElemFader.js  ?JavaScript-FX.com
* Created : 2001/08/31 
* Author  : Roy Whittle  (Roy@Whittle.com) www.Roy.Whittle.com 
* Purpose : To create a fading effect for any html element.
* History 
* Date         Version  Description 
* 2001-08-09	1.0	First version
* 2001-08-31	1.1	Code split - others became 
*					JSFX_FadingRollovers,
*                             JSFX_ImageFader,
*                             JSFX_ElemFader,
*					JSFX_ImageZoom.
* 2002-01-27	1.2	Completed development by converting to JSFX namespace
* 2002-02-21	1.3	Added JSFX.fadeUpElem JSFX.fadeDownElem
* 2002-01-29	1.4	Make "fade" a seperate object of "elem"
* 2002-03-12	1.5	Added an auto fade up/down for images 
*					with class imageFader
* 2002-08-29	1.6	Thanks to piglet (http://homepage.ntlworld.com/thepiglet/)
*				I now have a partial fix for NS7 and Mozilla 1.1.
* 2003-10-02	1.7	First version of ElemFader
***********************************************************************/ 

if(!window.JSFX)
	JSFX=new Object();

JSFX.FadeElemMinOpacity = 50;
JSFX.FadeElemAutoUp	 = 20;
JSFX.FadeElemAutoDown   = 20;
JSFX.FadeElemSavedOver  = null;
JSFX.FadeElemSavedOut   = null;
JSFX.FadeElemIdIndex   = 0;
document.write('<STYLE TYPE="text/css">.elemFader{ position:relative; filter:alpha(opacity='+JSFX.FadeElemMinOpacity+'); -moz-opacity:'+JSFX.FadeElemMinOpacity/101+'}</STYLE>');
document.write('<STYLE TYPE="text/css">.elemFader0{ position:relative; filter:alpha(opacity=0); -moz-opacity:0.0}</STYLE>');
document.write('<STYLE TYPE="text/css">.elemFader1{ position:relative; filter:alpha(opacity=10); -moz-opacity:0.1}</STYLE>');
document.write('<STYLE TYPE="text/css">.elemFader2{ position:relative; filter:alpha(opacity=20); -moz-opacity:0.2}</STYLE>');
document.write('<STYLE TYPE="text/css">.elemFader3{ position:relative; filter:alpha(opacity=30); -moz-opacity:0.3}</STYLE>');
document.write('<STYLE TYPE="text/css">.elemFader4{ position:relative; filter:alpha(opacity=40); -moz-opacity:0.4}</STYLE>');
document.write('<STYLE TYPE="text/css">.elemFader5{ position:relative; filter:alpha(opacity=50); -moz-opacity:0.5}</STYLE>');
document.write('<STYLE TYPE="text/css">.elemFader6{ position:relative; filter:alpha(opacity=60); -moz-opacity:0.6}</STYLE>');
document.write('<STYLE TYPE="text/css">.elemFader7{ position:relative; filter:alpha(opacity=70); -moz-opacity:0.7}</STYLE>');
document.write('<STYLE TYPE="text/css">.elemFader8{ position:relative; filter:alpha(opacity=80); -moz-opacity:0.8}</STYLE>');
document.write('<STYLE TYPE="text/css">.elemFader9{ position:relative; filter:alpha(opacity=90); -moz-opacity:0.9}</STYLE>');
/*******************************************************************
*
* Function    : actionOnMouseOver
*
* Description : Called automatically whenever an element in the
*			document is "mousedOver". It checks to see if the
*			element has the className == "elemFader" and if so
*			starts fading up the element.
*
*****************************************************************/
JSFX.fadeElem_actionOnMouseOver = function(e)
{
	srcElement=e ? e.target : event.srcElement;
	
	if(srcElement.className && srcElement.className.indexOf("elemFader") != -1)
		JSFX.fadeUp(srcElement);

	/*** If the document already had an onMouseOver handler, call it ***/
	if(JSFX.FadeElemSavedOver != null)
		JSFX.FadeElemSavedOver(e);
}

/*******************************************************************
*
* Function    : actionOnMouseOut
*
* Description : Called automatically whenever an element in the
*			document is "mousedOut". It checks to see if the
*			element has the className == "elemFader" and if so
*			starts fading down the element.
*
*****************************************************************/
JSFX.fadeElem_actionOnMouseOut = function(e)
{
	srcElement=e ? e.target : event.srcElement;

	if(srcElement.className && srcElement.className.indexOf("elemFader") != -1)
		JSFX.fadeDown(srcElement);
	
	/*** If the document already had an onMouseOut handler, call it ***/
	if(JSFX.FadeElemSavedOut != null)
		JSFX.FadeElemSavedOut(e);
}
/*******************************************************************
*
* Function    :	fadeElemAuto
*
* Parameters  :	minOpacity	- Minimum opacity to fade down to.
*			stepUp	- fade up speed 	- larger = faster.
*			stepDown 	- fade down speed	- larger = faster.
*
* Description :	Saves the documents original mousOver/Out handlers
*			and installs the actionMouseOver/Out handlers
*			of JSFX.fadeElem
*
*****************************************************************/
JSFX.fadeElemAuto = function(minOpacity, stepUp, stepDown)
{
	if(minOpacity)
		JSFX.FadeElemMinOpacity = minOpacity;
	if(stepUp)
		JSFX.FadeElemAutoUp	= stepUp;
	if(stepDown)
		JSFX.FadeElemAutoDown	= stepDown;

	/*** Save the original document mouseOver/Out events ***/
	JSFX.FadeElemSavedOver = document.onmouseover;
	JSFX.FadeElemSavedOut  = document.onmouseout;

	document.onmouseover	= JSFX.fadeElem_actionOnMouseOver ;
	document.onmouseout	= JSFX.fadeElem_actionOnMouseOut ;
}
/*******************************************************************
*
* Function    : fadeUpElem
*
* Description : Finds the elem in the document and calls JSFX.fadeUp
*
*****************************************************************/
JSFX.fadeUpElem = function(elemName, step)
{
	if(document.layers || window.opera)
		return;

	elem = document.getElementById(elemName);
	if(elem)
		JSFX.fadeUp(elem, step);
}
/*******************************************************************
*
* Function    : fadeUp
*
* Description : This function is based on the turn_on() function
*		      of animate2.js (animated rollovers from www.roy.whittle.com).
*		      Each fading elem object is given a state. 
*			OnMouseOver the state is switched depending on the current state.
*			Current state -> Switch to
*			===========================
*			null		->	OFF.
*			OFF		->	FADE_UP
*			FADE_DOWN	->	FADE_UP
*			FADE_UP_DOWN->	FADE_UP
*****************************************************************/
JSFX.fadeUp = function(elem, step)
{

	if(elem)
	{
		if(!step) step=JSFX.FadeElemAutoUp;

		if(elem.fade == null)
		{
			elem.fade = new Object();
			elem.fade.state	 = "OFF";
			elem.fade.upStep	 = step;
			elem.fade.downStep  = step;
			elem.fade.minOpacity  = JSFX.FadeElemMinOpacity;
			elem.fade.index = elem.fade.minOpacity;
			elem.animate=JSFX.FadeElemAnimation;
			elem.fadeId="JSFX_FadeElem" + JSFX.FadeElemIdIndex++;
			window[elem.fadeId] = elem;

			if(elem.filters)
				elem.fade.minOpacity = elem.filters.alpha.opacity;
			else
				elem.fade.minOpacity = document.defaultView.getComputedStyle(elem,'').getPropertyValue('-moz-opacity') * 100;
			
		}
		if(elem.fade.state == "OFF")
		{
			elem.fade.upStep  = step;
			elem.fade.state = "FADE_UP";
			elem.animate();
		}
		else if( elem.fade.state == "FADE_UP_DOWN"
			|| elem.fade.state == "FADE_DOWN")
		{
			elem.fade.upStep  = step;
			elem.fade.state = "FADE_UP";
		}
	}
}
/*******************************************************************
*
* Function    : fadeDownElem
*
* Description : Finds the elem in the document and calls JSFX.fadeDown
*
*****************************************************************/
JSFX.fadeDownElem = function(elemName, step)
{
	if(document.layers || window.opera)
		return;

	elem = document.getElementById(elemName);
	if(elem)
		JSFX.fadeDown(elem, step);
}
/*******************************************************************
*
* Function    : fadeDown
*
* Description : This function is based on the turn_off function
*		      of animate2.js (animated rollovers from www.roy.whittle.com).
*		      Each zoom object is given a state. 
*			OnMouseOut the state is switched depending on the current state.
*			Current state -> Switch to
*			===========================
*			ON		->	FADE_DOWN.
*			FADE_UP	->	FADE_UP_DOWN.
*****************************************************************/
JSFX.fadeDown = function(elem, step)
{
	if(elem)
	{
		if(!step) step=JSFX.FadeElemAutoDown;

		if(elem.fade.state=="ON")
		{
			elem.fade.downStep  = step;
			elem.fade.state="FADE_DOWN";
			elem.animate();
		}
		else if(elem.fade.state == "FADE_UP")
		{
			elem.fade.downStep  = step;
			elem.fade.state="FADE_UP_DOWN";
		}
	}
}
/*******************************************************************
*
* Function    : FadeElemAnimation
*
* Description : This function is based on the Animate function
*		    of animate2.js (animated rollovers from www.roy.whittle.com).
*		    Each object has a state. This function
*		    modifies each object and (possibly) changes its state.
*****************************************************************/
JSFX.FadeImageAnimation = function()
{
	JSFX.FadeElemRunning = false;
	for(i=0 ; i<document.elems.length ; i++)
	{
		var elem = document.elems[i];
		if(elem.fade)
		{
			if(elem.fade.state == "FADE_UP")
			{
				elem.fade.index+=elem.fade.upStep;
				if(elem.fade.index > 100)
					elem.fade.index = 100;

				if(elem.filters)
					elem.filters.alpha.opacity = elem.fade.index;
				else
					elem.style.MozOpacity = elem.fade.index/101;

				if(elem.fade.index == 100)
					elem.fade.state="ON";
				else
					JSFX.FadeElemRunning = true;
			}
			else if(elem.fade.state == "FADE_UP_DOWN")
			{
				elem.fade.index+=elem.fade.upStep;
				if(elem.fade.index > 100)
					elem.fade.index = 100;

				if(elem.filters)
					elem.filters.alpha.opacity = elem.fade.index;
				else
					elem.style.MozOpacity = elem.fade.index/101;
	
				if(elem.fade.index == 100)
					elem.fade.state="FADE_DOWN";
				JSFX.FadeElemRunning = true;
			}
			else if(elem.fade.state == "FADE_DOWN")
			{
				elem.fade.index-=elem.fade.downStep;
				if(elem.fade.index < elem.fade.minOpacity)
					elem.fade.index = elem.fade.minOpacity;

				if(elem.filters)
					elem.filters.alpha.opacity = elem.fade.index;
				else
					elem.style.MozOpacity = elem.fade.index/101;

				if(elem.fade.index == elem.fade.minOpacity)
					elem.fade.state="OFF";
				else
					JSFX.FadeElemRunning = true;
			}
		}
	}
	/*** Check to see if we need to animate any more frames. ***/
	if(JSFX.FadeElemRunning)
		setTimeout("JSFX.FadeElemAnimation()", 10);
}

/*******************************************************************
*
* Function    : FadeElemAnimation
*
* Description : This function is based on the Animate function
*		    of animate2.js (animated rollovers from www.roy.whittle.com).
*		    Each object has a state. This function
*		    modifies each object and (possibly) changes its state.
*****************************************************************/
JSFX.FadeElemAnimation = function()
{
	var fadeElemRunning = false;
	var elem = this;
	if(elem.fade)
	{
		if(elem.fade.state == "FADE_UP")
		{
			elem.fade.index+=elem.fade.upStep;

			if(elem.fade.index > 100)
				elem.fade.index = 100;

			if(elem.filters)
				elem.filters.alpha.opacity = elem.fade.index;
			else
				elem.style.MozOpacity = elem.fade.index/101;

			if(elem.fade.index == 100)
				elem.fade.state="ON";
			else
				fadeElemRunning = true;
		}
		else if(elem.fade.state == "FADE_UP_DOWN")
		{
			elem.fade.index+=elem.fade.upStep;
			if(elem.fade.index > 100)
				elem.fade.index = 100;

			if(elem.filters)
				elem.filters.alpha.opacity = elem.fade.index;
			else
				elem.style.MozOpacity = elem.fade.index/101;
	
			if(elem.fade.index == 100)
				elem.fade.state="FADE_DOWN";
			fadeElemRunning = true;
		}
		else if(elem.fade.state == "FADE_DOWN")
		{
			elem.fade.index-=elem.fade.downStep;
			if(elem.fade.index < elem.fade.minOpacity)
				elem.fade.index = elem.fade.minOpacity;

			if(elem.filters)
				elem.filters.alpha.opacity = elem.fade.index;
			else
				elem.style.MozOpacity = elem.fade.index/101;

			if(elem.fade.index == elem.fade.minOpacity)
				elem.fade.state="OFF";
			else
				fadeElemRunning = true;
		}
	}
	/*** Check to see if we need to animate any more frames. ***/
	if(fadeElemRunning)
		setTimeout("window['"+elem.fadeId+"'].animate()", 10);
}

