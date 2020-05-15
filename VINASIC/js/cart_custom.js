$(document).ready(function()
{
	"use strict";
	var menuActive = false;
	var header = $('.header');
	setHeader();
	initCustomDropdown();
	initPageMenu();
	$(window).on('resize', function()
	{
		setHeader();
	});
	function setHeader()
	{
		if(window.innerWidth > 991 && menuActive)
		{
			closeMenu();
		}
	}
	function initCustomDropdown()
	{
		if($('.custom_dropdown_placeholder').length && $('.custom_list').length)
		{
			var placeholder = $('.custom_dropdown_placeholder');
			var list = $('.custom_list');
		}
		$('.custom_list a').on('click', function (ev)
		{
			ev.preventDefault();
			var index = $(this).parent().index();

			placeholder.text( $(this).text() ).css('opacity', '1');

			if(list.hasClass('active'))
			{
				list.removeClass('active');
			}
			else
			{
				list.addClass('active');
			}
		});
	}
	function initPageMenu()
	{
		if($('.page_menu').length && $('.page_menu_content').length)
		{
			var menu = $('.page_menu');
			var menuContent = $('.page_menu_content');
			var menuTrigger = $('.menu_trigger');
			//Open / close page menu
			menuTrigger.on('click', function()
			{
				if(!menuActive)
				{
					openMenu();
				}
				else
				{
					closeMenu();
				}
			});
			//Handle page menu
			if($('.page_menu_item').length)
			{
				var items = $('.page_menu_item');
				items.each(function()
				{
					var item = $(this);
					if(item.hasClass("has-children"))
					{
						item.on('click', function(evt)
						{
							evt.preventDefault();
							evt.stopPropagation();
							var subItem = item.find('> ul');
						    if(subItem.hasClass('active'))
						    {
						    	subItem.toggleClass('active');
								TweenMax.to(subItem, 0.3, {height:0});
						    }
						    else
						    {
						    	subItem.toggleClass('active');
						    	TweenMax.set(subItem, {height:"auto"});
								TweenMax.from(subItem, 0.3, {height:0});
						    }
						});
					}
				});
			}
		}
	}
	function openMenu()
	{
		var menu = $('.page_menu');
		var menuContent = $('.page_menu_content');
		TweenMax.set(menuContent, {height:"auto"});
		TweenMax.from(menuContent, 0.3, {height:0});
		menuActive = true;
	}
	function closeMenu()
	{
		var menu = $('.page_menu');
		var menuContent = $('.page_menu_content');
		TweenMax.to(menuContent, 0.3, {height:0});
		menuActive = false;
	}
});

var app = angular.module('myApp', []);
angular.module('myApp').directive('numOnly', numOnly);
function numOnly() {
	var directive = {
		restrict: 'A',
		scope: {
			ngModel: '=ngModel'
		},
		link: link
	};
	return directive;
	function link(scope, element, attrs) {
		scope.$watch('ngModel', function (newVal, oldVal) {
			var arr = String(newVal).split('');
			if (arr.length === 0) return;
			if (arr.length === 1 && (arr[0] === '-' || arr[0] === '.')) return;
			if (isNaN(newVal)) {
				scope.ngModel = oldVal;
			}
		});
	}
}
app.controller('myCtrl', function ($scope, $location, $http) {
	$scope.onlyNumbers = /^\d+$/;
	$scope.stickers = [];
	$scope.price = 0;
	$scope.subTotal = 0;
	$scope.square = 0;
	$scope.shoppingCart = {
		material: {},
		width: '',
		height: '',
		quantity: '',
		machining: {},
		cut: {},
	};
	$scope.selectedMaterial = '';
	$scope.options = [
		{ value: 'decalsua', label: 'Decal sữa' },
		{ value: 'decaltrong', label: 'Decal trong' },
		{ value: 'decalgiay', label: 'Decal giấy' },
		{ value: 'decalkraft', label: 'Decal Kraft' },
		{ value: 'decalxi', label: 'Decal xi bạc' },
		{ value: 'decal7mau', label: 'Decal 7 màu' },
		{ value: 'tembe', label: 'Tem bảo hành, tem vỡ,tem bế' },
		{ value: 'decalsuacc', label: 'Decal sữa cao cấp' }
	];
	$scope.options1 = [
		{ value: 'canmang', label: 'Cán màng' },
		{ value: 'khongcanmang', label: 'Không cán màng' },
	];
	$scope.options2 = [
		{ value: 'bethang', label: 'Bế thẳng' },
		{ value: 'bedd', label: 'Bế đặc biệt(hình tròn, hình khó)' },
	];
	
	$scope.shoppingCart.material = $scope.options[0];
	$scope.shoppingCart.machining = $scope.options1[0];
	$scope.shoppingCart.cut = $scope.options2[0];

	$.ajax({
		url: "/Sticker/GetAllSticker",
		type: 'post',
		contentType: 'application/json',
		success: function (result) {
			$scope.stickers = result.Records;
		}
	});
	$scope.materialChange = function () {
		if ($scope.shoppingCart.material.value == 'tembe' || $scope.shoppingCart.material.value == 'decal7mau' || $scope.shoppingCart.material.value == 'decalxi' || $scope.shoppingCart.material.value == 'decalkraft') {
			$scope.options1 = [
				{ value: 'khongcanmang', label: 'Không cán màng' },
			];
		} else {
			$scope.options1 = [
				{ value: 'canmang', label: 'Cán màng' },
				{ value: 'khongcanmang', label: 'Không cán màng' },
			];
		}
		$scope.shoppingCart.machining = $scope.options1[0];
	}
	var calculateTotals = function () {
		var sticker = {};
		var width = isNaN(parseFloat($scope.shoppingCart.width) )? 0 : parseFloat($scope.shoppingCart.width);
		var height = isNaN(parseFloat($scope.shoppingCart.height)) ? 0 : parseFloat($scope.shoppingCart.height);
		var quantity = isNaN(parseFloat($scope.shoppingCart.quantity))? 0 : parseFloat($scope.shoppingCart.quantity);
		if (width >0 && height>0 && quantity >0) {
			$scope.square = (width * height * quantity) / 1000000;
			sticker = $scope.stickers.find(obj => {
				return obj.Code === $scope.shoppingCart.material.value && obj.SquareTo == 0
			});
			if ($scope.square < sticker.SquareFrom) {
				sticker = $scope.stickers.find(obj => {
					return obj.Code === $scope.shoppingCart.material.value && obj.SquareFrom <= $scope.square && $scope.square < obj.SquareTo
				});
			}
			var curtainPrice = 0;
			var nonCurtainPrice = 0;
			if ($scope.shoppingCart.machining.value == "canmang" ) {
				curtainPrice = sticker.CurtainPrice * $scope.square;
				$scope.price = sticker.CurtainPrice;
			} else {
				curtainPrice = 0;
			}
			if ($scope.shoppingCart.machining.value == "khongcanmang") {
				var nonCurtainPrice = sticker.NoneCurtainPrice * $scope.square;
				$scope.price = sticker.NoneCurtainPrice;
			} else {
				nonCurtainPrice = 0;
			}
			var specialPrice = $scope.shoppingCart.cut.value == "bedd" ? sticker.SpecialPrice * $scope.square : 0;
			var noneDefaultPrice = curtainPrice + nonCurtainPrice + specialPrice;
			if (sticker.DefaultPrice) {
				$scope.subTotal = sticker.DefaultPrice;
				$scope.price = sticker.DefaultPrice;
			} else {
				$scope.subTotal = noneDefaultPrice;
			}
		}
	};
	$scope.$watch('shoppingCart', calculateTotals, true);

});