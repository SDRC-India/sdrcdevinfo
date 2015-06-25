function scolexp(object) {
    if (object.src.indexOf("minus.png") > 1) {
        //Collapse Data Div
        var Id = object.id.substr(3, object.id.length);
        di_jq("#dv" + Id).hide("slow");
        object.src = object.src.replace("minus.png", "plus.png");        
    }
    else {
        //Expand Data Div
        var Id = object.id.substr(3, object.id.length);
        di_jq("#dv" + Id).show("slow");
        object.src = object.src.replace("plus.png", "minus.png");        
    }
}

function scolexpinner(object) {
    if (object.src.indexOf("btm_arrow.png") > 1) {
        //Collapse Data Div
        var Id = object.id.substr(3, object.id.length);
        di_jq("#dv" + Id).hide("slow");
        object.src = object.src.replace("btm_arrow.png", "st_arrow.png");
    }
    else {
        //Expand Data Div
        var Id = object.id.substr(3, object.id.length);
        di_jq("#dv" + Id).show("slow");
        object.src = object.src.replace("st_arrow.png", "btm_arrow.png");
    }
}

function scolexpdv(object) {
    var Id = object.id.substr(3, object.id.length);
    if (di_jq("#img" + Id).attr('src').indexOf("minus.png") > 1) {
        //Collapse Data Div        
        di_jq("#dv" + Id).hide("slow");
        di_jq("#img" + Id).attr('src', di_jq("#img" + Id).attr('src').replace("minus.png", "plus.png"));
    }
    else {
        //Expand Data Div        
        di_jq("#dv" + Id).show("slow");
        di_jq("#img" + Id).attr('src', di_jq("#img" + Id).attr('src').replace("plus.png", "minus.png"));     
    }
}

function scolexdvinner(object) {
    var Id = object.id.substr(3, object.id.length);
    if (di_jq("#img" + Id).attr('src').indexOf("btm_arrow.png") > 1) {
        //Collapse Data Div        
        di_jq("#dv" + Id).hide("slow");
        di_jq("#img" + Id).attr('src', di_jq("#img" + Id).attr('src').replace("btm_arrow.png", "st_arrow.png"));
    }
    else {
        //Expand Data Div        
        di_jq("#dv" + Id).show("slow");
        di_jq("#img" + Id).attr('src', di_jq("#img" + Id).attr('src').replace("st_arrow.png", "btm_arrow.png"));
    }
}