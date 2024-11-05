(function () {
    'use strict';

    angular
        .module('app')
        .controller('BillDeskPayment', BillDeskPayment);

    BillDeskPayment.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','Excel', '$timeout', 'superCache'] 

    function BillDeskPayment($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, superCache) {
      
        $scope.formData = {
            redirectUrl: 'https://pgi.billdesk.com/pgidsk/PGIMerchantPayment',
            redirectMethod: 'POST',
            redirectData: {
                msg: "BGHSBLORE|ARP10234|NA|2.00|NA|NA|NA|INR|NA|R|bghsblore|NA|NA|F|NA|NA|NA|NA|NA|NA|NA|https://stagingcampusux.azurewebsites.net/api/FeeOnlinePayment/paymentresponse|087D178B8CF9B943D657EAF56C4DB1817B0B8753F3E7AFA5F21E7A6A20FEA366"
            }
        };

        $scope.pay = function () {
            var data = {
            }
            apiService.create("FeeOnlinePayment/billdeskPayment/", data).
              then(function (promise) {
              });
            $scope.$broadcast('gateway.redirect');
        }
      
    }
})();
// Billdesk Payment integration code starts.
app.directive('autoSubmitForm', ['$interpolate', function ($interpolate) {
    return {
        replace: true,
        scope: {
            formData: '='
        },
        template: '',
        link: function ($scope, element, $attrs) {
            $scope.$on($attrs['event'], function (event, data) {
                var form = $interpolate('<form action="{{formData.redirectUrl}}" method="{{formData.redirectMethod}}"><div><input name="msg"  type="hidden" value="{{formData.redirectData.msg}}"/></div></form>')($scope);
                 var e1 = angular.element(document.getElementById("payment"));
                jQuery(form).appendTo(e1).submit();
            })
        }
    }
}]);
// Billdesk Payment integration code ends.
