'use strict';

angular.module("app").service("locations-service", function(){

	var locationPartialsPath = "partials/data/locations/"

	function generatePartialPath(str){
		return locationPartialsPath + str + ".html";
	}

	this.planets = [
	{
		name: "Zakaas"
		partial: generatePartialPath("zakaas");
	},{
		name: "Shauraaj"
		partial: generatePartialPath("shauraaj");
	},{
		name: "Argatha"
		partial: generatePartialPath("argatha");
	},{
		name: "Vrimsh"
		partial: generatePartialPath("vrimsh");
	},{
		name: "Saedrun"
		partial: generatePartialPath("saedrun");
	},{
		name: "Karkirras"
		partial: generatePartialPath("karkirras");
	},{
		name: "Arrikshar"
		partial: generatePartialPath("arrikshar");
	},{
		name: "Varrazaar"
		partial: generatePartialPath("varrazaar");
	},{
		name: "Sharaag"
		partial: generatePartialPath("sharaag");
	}];

});