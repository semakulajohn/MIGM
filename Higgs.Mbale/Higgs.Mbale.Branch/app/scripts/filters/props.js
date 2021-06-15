/**
 *
 * propsFilter
 *
 */

angular
    .module('homer')
    .filter('propsFilter', propsFilter)

function propsFilter(){
    return function(items, props) {
        var out = [];

        if (angular.isArray(items)) {
            items.forEach(function(item) {
                var itemMatches = false;

                var keys = Object.keys(props);
                for (var i = 0; i < keys.length; i++) {
                    var prop = keys[i];
                    var text = props[prop].toLowerCase();
                    if (item[prop].toString().toLowerCase().indexOf(text) !== -1) {
                        itemMatches = true;
                        break;
                    }
                }

                if (itemMatches) {
                    out.push(item);
                }
            });
        } else {
            // Let the output be the input untouched
            out = items;
        }

        return out;
    }
}


angular
    .module('homer')
    .filter('customCurrency', function ($filter) {
        //return function (amount, currencySymbol) {
            return function (amount) {
        var currency = $filter('number');
        var word = amount.toString();
        if(word.charAt(0) === "-"){
        //if (amount.charAt(0) === "-") {
           // return currency(amount, currencySymbol)
            return currency(amount)
              .replace("(", "-")
              .replace(")", "");
        }

        //return currency(amount, currencySymbol);
        return currency(amount);
    };
});