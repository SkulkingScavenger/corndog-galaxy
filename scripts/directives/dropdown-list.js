/*
 * Author: Bipin Bihari Pradhan
 *
 * Directive Name: synapse-dropdown-list
 *
 * Directive for showing a drop down list in synapse style

 *
	<div style="display:inline-block" ng-show="showTimeoutList"
		ng-model="data.timeOut"
		synapse-dropdown-list
		name="'timeOut'"
		data-list="data.timeoutList"
		data-list-item-caption-attr="'caption'"
		data-list-item-value-attr="'value'"
		data-selected-item-value="data.system_inactivity_timeout"
		data-on-select="setTimeOut",
		data-rows="data.timeoutList.length"
		data-cols="10"
		></div>
 */

"use strict";

angular.module('synapseWebApp')

.directive( 'synapseDropdownList',

function($timeout) {
        var instanceCount = 0;
	return {
		restrict: 'A', // Only to be used as an attribute

		require: '?ngModel', // ngModalController for setting dirty and pristine flags

		scope : {
			elementName: "=name",      // for naming the html input element
			selectedItem: '=ngModel',   // For storing / showing selected item
			list : '=', 		       // Name of the attribute holding the list of items for dropdown
			listItemCaptionAttr: '=',  // Name of the attribute holding the caption to be displayed
			listItemValueAttr: '=',    // Name of the attribute whose value is to be bound
			selectedItemValue: '=',    // for showing the selected item as selected in the dropdown
            selectedItemCaption: '=',    // for showing the selected item as selected in the dropdownbuttonCaptionAttr: '=', //use only if caption shown on the button should vary from the caption shown in the dropdown
			rows: '=',
			cols: '=',
            dropdownWidth: '=',
			onSelect : '=',
			contextObject: '=',
			data : '=',
			defaultText : '=',
			listType : "=",
			disabled:'=',
			totalDataCount:'=',
			height: '='
		},

		templateUrl : '/partials/directives/dropdown-list.html',

		link: function (scope, element, attrs, ngModel) {
                    scope.id='dropdownMenu-list' + instanceCount++;
                    if(attrs.role){
                        scope.role = attrs.role;
                        element.removeAttr('role');
                    }
                    else{
                        scope.role = 'listbox';
                    }
                    if (scope.role === "listbox") {
                        scope.itemRole = "option";
                    }
                    else if(scope.role === "menu"){
                        scope.itemRole = "menuitem";
                    }
                    if(attrs.tabindex) {
											scope.tabIndex = attrs.tabindex;
											element.attr('tabindex', '');
										}else {
											scope.tabIndex = '0';
										}




			if (!angular.isDefined( scope.rows)) {

				if (angular.isDefined( scope.list) && angular.isDefined( scope.list.length)) {

					scope.lines = Math.min( 4, scope.list.length );
				} else {
					scope.lines = 4;
				}
			} else {

				scope.lines = parseInt(scope.rows);
			}

            scope.customStyle = {};

			scope.customWidth = "";

			if (angular.isDefined( scope.cols) && !angular.isDefined( scope.dropdownWidth)) {

				scope.cols = parseInt(scope.cols);

				var fontRatio = 0.5;
				var widthValue = ((scope.cols)*fontRatio) * 16;
                if(widthValue > 302){
                    widthValue = "302px";
                    scope.customStyle['min-width'] = '300px';
                }else{
                    scope.customStyle['min-width'] = (widthValue -2) +'px';
                }

				scope.customWidth = { width: widthValue};
			} else if (angular.isDefined( scope.dropdownWidth)) {
                scope.customStyle['min-width'] = scope.dropdownWidth;
                scope.customWidth = {width: scope.dropdownWidth};
            }


			scope.$watch( 'list', function (newValue) {

				renderSelectedItem();
			},true);


			scope.$watch( 'rows', function (newValue) {
				if(angular.isDefined(scope.height)){
					scope.customStyle['max-height'] = scope.height;
				}else {
					scope.customStyle['max-height'] = "350px";
				}
				scope.customStyle['height'] = "auto";

			});

			scope.$watch( 'selectedItemValue', function (newValue) {

				renderSelectedItem();
			});

			function renderSelectedItem () {

				if (angular.isDefined( scope.selectedItemValue)) {

					if ( angular.isDefined( scope.list) ) {

						for (var i=0; i < scope.list.length; i++) {

							if (scope.list[i][scope.listItemValueAttr] == scope.selectedItemValue) {

								scope.selectedItem = scope.list[i];

								break;
							}
						}

						if(scope.selectedItem){

						}
					}
				}
			}

			renderSelectedItem();

			function getItem (item) {

				return item;
			}

			function getItemCaption (item) {

				if ( angular.isDefined( item) ) {

					return item[scope.listItemCaptionAttr];
				}

				return item;
			}

			function getItemValue (item) {

				return item[scope.listItemValueAttr];
			}

			function getButtonCaption (item) {

				if ( angular.isDefined( item) ) {

					return item[scope.buttonCaptionAttr];
				}

				return item;
			}

			var caption = angular.isDefined( scope.listItemCaptionAttr)? getItemCaption: getItem;
			var value = angular.isDefined( scope.listItemCaptionAttr)? getItemValue: getItem;
			var buttonCaption = angular.isDefined( scope.buttonCaptionAttr)? getButtonCaption: getItem;

			scope.getCaption = function (listItem, isButtonCaption) {
				if(isButtonCaption && angular.isDefined(scope.buttonCaptionAttr)){
					return buttonCaption(listItem);
				} else {
					if (listItem != undefined && listItem != null && ((typeof listItem === 'object' && Object.keys(listItem).length > 0) || typeof listItem !== 'object')) {
						return caption(listItem);
					} else {
						return angular.isDefined(scope.defaultText) ? scope.defaultText : '';
					}
				}
			};

			scope.getValue = function (listItem) {

				return value( listItem);
			};

			scope.isSubTitle = function (listItem) {

				return (value( listItem) === null);
			};

      scope.getSmallestItem = function() {
        var smallestItem = null;
        if (scope.list[0] > 0) {
          smallestItem = scope.list[0];
        }
        return smallestItem;
      };

      scope.isButtonDisable = function(){

        if (angular.isDefined(scope.disabled) && scope.disabled) {
          return true;
        }

        if ( !angular.isDefined( scope.list) || !angular.isDefined( scope.list.length) ) {
          return true;
        }else if (scope.getSmallestItem() !== null) {
          var smallestItem = scope.getSmallestItem();
          if (scope.totalDataCount) {
						return (scope.totalDataCount <= smallestItem);
          }else{
          	return false;
					}
        } else {
          return false;
        }

				if (angular.isDefined(scope.disabled)) {

					return ((scope.list.length <= 0 || scope.disabled) ? true:false);
				}

				return (scope.list.length > 0 ? false:true);
			};

			scope.isSelected = function (listItem) {

				return !scope.isSubTitle( listItem) && (listItem === scope.selectedItem);
			};

      scope.setFocus = function (isLast, e, currentIndex) {
        if (isLast) {
          if (((e.keyCode == 9) || (e.keyCode == 40)) && !e.shiftKey) {
            shiftFocus(0, e);
          }
        } else {
          if (e.keyCode == 40) {
            shiftFocus((currentIndex + 1), e);
          }
        }
        if (e.keyCode == 38) {
          shiftFocus((currentIndex - 1), e);
        }
      };

      function shiftFocus(element, e) {
        angular.element('#li' + scope.id + element).focus();
        e.preventDefault();
        return false;
      }

      scope.openDropdown = function () {
        var element = angular.element(document.activeElement).parent('div');
        if (element.hasClass('open')) {
          element.removeClass('open');
        } else {
          element.addClass('open');
          $timeout(function () {
            element.find('li').first().focus();
          }, 100);
        }
      };

      scope.closeDropdown = function ($event) {
        $timeout(function () {
          var element = angular.element($event.currentTarget).parent('ul');
          element.parent('div').removeClass('open');
          element.prev('button').focus();
        }, 100);
      };


      scope.selectItem = function (listItem,e,key,id) {

				if ( scope.isSubTitle( listItem) ) {
					return;
				}

				if (e == 'e'){
                                    $timeout(function() {
                                        var e1 = document.getElementById("li"+id + key)

                                        e = angular.element(e1)
                                        e.trigger('click');
                                    }, 10);

                                    return;
                                }

				scope.selectedItem = listItem;

				if (angular.isDefined( scope.selectedItemValue)) {

					scope.selectedItemValue = value( listItem);
				}

                                if (angular.isDefined( scope.selectedItemCaption)) {
                                    scope.selectedItemCaption = caption( listItem);
                                }

				if (angular.isDefined( ngModel)) {

					ngModel.$setPristine( false);
					ngModel.$modalValue = value( listItem);
				}

				if (angular.isDefined( scope.onSelect)) {

					$timeout( function () {
                        if ( angular.isDefined( scope.contextObject) ) {

                            scope.onSelect.apply( scope.contextObject, [ listItem, scope.data ]);
                        } else {
                            scope.onSelect( listItem, scope.data);
                        }
					}, 50);
				}

			};
		} // end of link function
	}; // end of return statement
});
