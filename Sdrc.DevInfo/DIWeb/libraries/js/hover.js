di_jq(document).ready(function() {	
	function megaHoverOver(){
		di_jq(this).find(".sub").stop().fadeTo('fast', 1).show();			
		//Calculate width of all ul's
		(function(di_jq) { 
			jQuery.fn.calcSubWidth = function() {
				rowWidth = 0;
				//Calculate row
				di_jq(this).find("ul").each(function() {					
					rowWidth += di_jq(this).width(); 
				});	
			};
		})(jQuery); 
		
		if ( di_jq(this).find(".row").length > 0 ) { //If row exists...
			var biggestRow = 0;	
			//Calculate each row
			di_jq(this).find(".row").each(function() {							   
				di_jq(this).calcSubWidth();
				//Find biggest row
				if(rowWidth > biggestRow) {
					biggestRow = rowWidth;
				}
			});
			//Set width
			di_jq(this).find(".sub").css({'width' :biggestRow});
			di_jq(this).find(".row:last").css({'margin':'0'});
			
		} else { //If row does not exist...
			
			di_jq(this).calcSubWidth();
			//Set Width
			di_jq(this).find(".sub").css({'width' : rowWidth});
			
		}		
	}
	
	function megaHoverOut(){ 
	  di_jq(this).find(".sub").stop().fadeTo('fast', 0, function() {
		  di_jq(this).hide(); 
		  //di_jq(this).slideToggle(); 
	  });
	}


	var config = {    
		 sensitivity: 2, // number = sensitivity threshold (must be 1 or higher)    
		 interval: 100, // number = milliseconds for onMouseOver polling interval    
		 over: megaHoverOver, // function = onMouseOver callbaCK (REQUIRED)    
		 timeout: 500, // number = milliseconds delay before onMouseOut    
		 out: megaHoverOut // function = onMouseOut callbaCK (REQUIRED)    
	};

	di_jq("li#dbMenu .sub").css({'opacity':'0'});
	di_jq("li#dbMenu").hoverIntent(config);	
});
