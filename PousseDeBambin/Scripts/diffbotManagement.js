$(function () {
    var searchAjax = null;
    // On init les boutons de recherche
    InitSearch();

    // Lorsque l'on clique sur le bouton d'arret de la recherche
    $("#btnStopCreateSearchObject").click(function () {
        if (searchAjax) {
            // On annule la requete de recherche
            searchAjax.abort();
            searchAjax = null;

            // On réaffiche le bouton de recherches
            SearchTerminate();
        }
    });

    // Lorsque l'on clique sur le bouton de recherche de l'objet en création
    $("#btnCreateSearchObject").click(function () {
        // On affiche le bouton pour stopper la recherche
        Search();

        // We get the website
        var website = $('#WebSite').val();

        // Loading
        searchAjax = $.ajax({
            url: 'http://api.diffbot.com/v2/product',
            crossDomain: true,
            data: { 'token': '2d5a54f7471c7958ae0c4d3dcdcd5592', 'url': website },
            dataType: 'json',
            type: 'GET',
            timeout: 8000,
            success: function (data) {
                try {
                    // Fill all variables with returned values
                    var amount = ('amount' in data.products[0].offerPriceDetails) ? data.products[0].offerPriceDetails.amount : '0';
                    var link = ('link' in data.products[0].media[0]) ? data.products[0].media[0].link : '';
                    var title = ('title' in data.products[0]) ? data.products[0].title : '';
                    var description = ('description' in data.products[0]) ? data.products[0].description : '';

                    // Fill all divs with the return of the data

                    // Normalisation du prix en FR
                    var amountNormalizedFr = normalize(amount);

                    // ImageUrl
                    $("#ImageUrl").val(link);
                    // Price
                    $("#Price").val(amountNormalizedFr);
                    // Title / Name of the product
                    $("#Name").val(title);
                    // Description
                    $("#Description").val(description);
                    // Ajout de l'image
                    $("#displayImage").attr("src", link);
                }
                catch (e) {
                    alert("Un problème est en cours avec la recherche automatique, veuillez réessayer plus tard :). En attendant, vous pouvez toujours entrer les informations à la main ;).");
                    console.log("Problème dans la récupération des données");
                    //TODO : Logguer l'erreur
                }

                SearchTerminate();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("Un problème est en cours avec la recherche automatique, veuillez réessayer plus tard :). En attendant, vous pouvez toujours entrer les informations à la main ;).");
                console.log("Problème dans le retour diffbot");
                SearchTerminate();
            }
        });

        return false;
    });

    function Search() {
        // On cache le bouton de recherche
        $('#btnCreateSearchObject').hide();
        // On affiche les champs remplis
        $('#giftInfosCreate').hide();
        // On affiche le bouton d'arret de recherche
        $('#btnStopCreateSearchObject').show();
    }

    function SearchTerminate() {
        // On affiche le bouton de recherche
        $('#btnCreateSearchObject').show();
        // On affiche les champs remplis
        $('#giftInfosCreate').show();
        // On cache le bouton d'arret de recherche
        $('#btnStopCreateSearchObject').hide();
    }

    function InitSearch() {
        // On affiche le bouton de recherche
        $('#btnCreateSearchObject').show();
        // On cache le bouton d'arret de recherche
        $('#btnStopCreateSearchObject').hide();
    }

    function normalize(amount) {
        return amount.toString().replace(".", ",");
    }
});