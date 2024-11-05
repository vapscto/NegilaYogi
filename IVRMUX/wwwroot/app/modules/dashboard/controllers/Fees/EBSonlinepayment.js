

(function () {
    'use strict';
    angular
.module('app')
.controller('EBSonlinepaymentController', EBSonlinepaymentController)

    EBSonlinepaymentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$window', 'FormSubmitter', 'superCache']
    function EBSonlinepaymentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $window, FormSubmitter, superCache) {

        $scope.loaddata = function () {
           
            var pageid = 2;
            apiService.getURI("EBSonlinepayment/getalldetails", pageid).
        then(function (promise) {
            
            var url = promise.paydet[0].v3URL;
            var method = 'POST';
            var params = {
                "secure_hash": promise.paydet[0].secure_hash,
                "account_id": promise.paydet[0].account_id,
                "channel": promise.paydet[0].channel,
                "currency": promise.paydet[0].currency,
                "reference_no": promise.paydet[0].reference_no,
                "amount": promise.paydet[0].amount,
                "ba_B25914CA4": promise.paydet[0].ba_B25914CA4,
                "ba_B259148DC": promise.paydet[0].ba_B259148DC,
                "split_profile_code": promise.paydet[0].split_profile_code,
                "description": promise.paydet[0].description,
                "name": promise.paydet[0].name,
                "address": promise.paydet[0].address,
                "city": promise.paydet[0].city,
                "state": promise.paydet[0].state,
                "postal_code": promise.paydet[0].postal_code,
                "country": promise.paydet[0].country,
                "email": promise.paydet[0].email,
                "phone": promise.paydet[0].phone,
                "mode": promise.paydet[0].mode,
                "return_url": promise.paydet[0].return_url,
                "ship_name": promise.paydet[0].ship_name,
                "ship_address": promise.paydet[0].ship_address,
                "ship_city": promise.paydet[0].ship_city,
                "ship_state": promise.paydet[0].ship_state,
                "ship_country": promise.paydet[0].ship_country,
                "ship_phone": promise.paydet[0].ship_phone,
                //  "algo":promise.paydet[0].algo,
                "ship_postal_code": promise.paydet[0].ship_postal_code,
                //  "name_on_card":promise.paydet[0].name_on_card,
                //  "card_number":promise.paydet[0].card_number,
                //  "card_expiry":promise.paydet[0].card_expiry,
                //  "card_cvv":promise.paydet[0].card_cvv,
            }
            FormSubmitter.submit(url, method, params);
        })           
        }
    }
})();