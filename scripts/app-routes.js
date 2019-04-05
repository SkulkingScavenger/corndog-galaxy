'use strict';

window.routes = {

	"/": {
	 	title :'Neoyahazeer Home',
		templateUrl: 'partials/pages/home.html',
		controller: 'home-ctrl',
	}, 
	"/locations": {
		title :'Neoyahazeer Locations',
		templateUrl: 'partials/locations.html',
		controller: 'regions-ctrl',
	},
	"/locations/:index": {
		title :'Neoyahazeer Locations',
		templateUrl: 'partials/location-info.html',
		controller: 'region-info-ctrl',
	},
	"/characters": {
		title :'Neoyahazeer Regions',
		templateUrl: 'partials/characters.html',
		controller: 'character-ctrl',
	},
	"/characters/:index": {
		title :'Neoyahazeer Characters',
		templateUrl: 'partials/character-info.html',
		controller: 'character-info-ctrl',
	},
	"/civilizations": {
		title :'Neoyahazeer Civilizations',
		templateUrl: 'partials/civilizations.html',
		controller: 'civilizations-ctrl',
	},
	"/civilizations/:index": {
		title :'Neoyahazeer Civilizations',
		templateUrl: 'partials/civilization-info.html',
		controller: 'civilization-info-ctrl',
	},
	"/species": {
		title :'Neoyahazeer Species',
		templateUrl: 'partials/species.html',
		controller: 'species-ctrl',
	},
	"/species/:index": {
		title :'Neoyahazeer Species',
		templateUrl: 'partials/species-info.html',
		controller: 'species-info-ctrl',
	},
	"/technologies": {
		title :'Neoyahazeer Technologies',
		templateUrl: 'partials/technologies.html',
		controller: 'technologies-ctrl',
	},
	"/technologies/:index": {
		title :'Neoyahazeer Technologies',
		templateUrl: 'partials/technologies-info.html',
		controller: 'technologies-info-ctrl',
	},
	"/mechanics": {
		title :'Neoyahazeer Mechanics',
		templateUrl: 'partials/mechanics.html',
		controller: 'mechanics-ctrl',
	},
	"/mechanics/:index": {
		title :'Neoyahazeer Mechanics',
		templateUrl: 'partials/mechanics-info.html',
		controller: 'mechanics-info-ctrl',
	},
};