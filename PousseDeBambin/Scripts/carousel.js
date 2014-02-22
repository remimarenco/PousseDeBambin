/**
 * Carousel Library
 *  - create a carousel with different effect
 *
 * Author: Jean-Christophe HAUTREUX
 * Version: 1.0
 *
 * -----------------------------------------------------------------------------------------------------------------------
 *
 * IMPORTANT: 
 *          to instantiate the Carousel, you can use:
 *              var yourCarousel = new Carousel();
 *              yourCarousel.construct(container, config);
 *          or more complete to obtain a Carousel with the capabilities of an external library (Prototype or JQuery):
 *              var yourCarousel = Carousel.create(container, config);
 *          or 
 *              container.setCarousel(config);
 *
 * Parameters:
 * - container [required]: the target element where the carousel will be displayed
 * 
 * - config [optional]: an object of config parameters
 *          possible entries:
 *              elements            : Array/Object containing each element of the carousel
 *                                      this parameter can be:
 *                                          - a simple object/array
 *                                          - a jquery/prototype list element
 *                                          - an array/object whose each entry is combine with an object like this:
 *                                              {tag: <the element>, param1: a user parameter, param2: an another user parameter, ...}
 *                                              all parameter other than the 'tag' is optional but passed to the event fallback functions
 *                                      if no elements provided, the carousel try to use the element directly into the container
 *              autostart           : true/false (default true) - to start automatically the switching
 *              autorotation        : true/false (default true) - to change automatically the element in first place
 *              widthElement        : number (default null) - the width of an element in pixel (if null, this value equal the width of the first element)
 *              heightElement       : number (default null) - the height of an element in pixel (if null, this value equal the height of the first element)
 *              displayedElement    : number (default null) - the number of element displayable simultaneously in the target container 
 *                                                            (if null, this value correspond to the division of the target container width by the widthElement)
 *              needMask            : true/false (default false) - defined if an element "mask" is added (to add a designed mask over the carousel) 
 *              navigation          : a sub-object of navigation configuration
 *                  type            : 'shortcut'/'thumbnail'/'closest' (default 'shortcut') - defined the type of navigation (for multi type use | between each value)
 *                                                shortcut - a link for each element
 *                                                thumbnail - a link for each element with the image
 *                                                closest - a link to access to the next element and an other to the previous
 *                  container       : a target container for shortcut link in case of shortuct navigation type
 *                  label           :
 *                      previous    : text (default 'Previous') - the text for the previous link in closest navigation type
 *                      next        : text (default 'Next') - the text for the newxt link in closest navigation type
 *                  thumbnails      : object of elements to use instead of thumbnails (same order that the elements)
 *              effect              : a sub-object of transition effect configuration
 *                  type            : name of the effect
 *                  params          : object of parameters to send to the effect (view effect documentation) 
 *              events              : an object of fallback function
 *                  beforenext      : launch just before the change of an element in favor of its following,
 *                  beforeprevious  : launch just before the change of an element in favor of its previous
 *                  change          : launch just after the change of an element
 *                  init            : launch just to the end of the _initialization of the carousel
 *                  aftereffect     : launch when the effect is finished
 *        you can define another parameters which can be retrieve into the userConfig attribute
 */
var Carousel = function(){};

/**
 * Function to use to obtain a good instance of the Carousel
 */
Carousel.create = function(container, config, carouselType) {
    var carousel = false;
    if( carouselType == undefined )
    {
        carouselType = Carousel.getLibraryClass();
        carousel = Carousel.create(container, config, carouselType);
    }
    else
    {
        carousel = new carouselType.prototype.constructor();
        // by inheritance, each instance of a Carousel Class child is an instance of Carousel
        if( carousel instanceof Carousel )
            carousel.construct(container, config);
    }

    return carousel;
}

/**
 * detect if a JS library is loaded
 */
Carousel.detectUsedLibrary = function()
{
    if( typeof jQuery !== 'undefined')
        return 'jquery';
    // for prototype, the scriptaculous Effect is also needed
    else if (typeof Prototype !== 'undefined' && typeof Effect !== 'undefined' )
        return 'prototype';
    else
        return null;
}

/**
 * return the Carousel Class based on present library
 */
Carousel.getLibraryClass = function() {
    var carouselType;
    switch(Carousel.detectUsedLibrary())
    {
        case 'jquery':
            carouselType = (JQueryCarousel != undefined) ? JQueryCarousel : Carousel;
            break;
        case 'prototype':
            carouselType = (PrototypeCarousel != undefined) ? PrototypeCarousel : Carousel;
            break;
        default:
            carouselType = Carousel;
    }
    return carouselType;
}

/**
 * to easily extend the carousel 
 */
Carousel.extend = function(parent){
    if( parent == undefined )
        parent = Carousel.getLibraryClass();
    
    var prototype = new parent.prototype.constructor();
    if( (prototype instanceof Carousel) == false )
    {
        parent = Carousel.getLibraryClass();
        prototype = new parent.prototype.constructor();
    }

    var extendingCarousel = function(){
        parent.prototype.constructor.call(this, arguments);
        // to avoid each instance has the same reference to "object"
        for(attrname in this)
        {
            if( typeof(this[attrname]) == 'object' )
                this[attrname] = Carousel.clone(this[attrname]);
        }
    };
    extendingCarousel.prototype = Carousel.clone(prototype);
    extendingCarousel.prototype.constructor = extendingCarousel;
    extendingCarousel.effects = {};
    for(var effectName in parent.prototype.constructor.effects)
    {
        extendingCarousel.effects[effectName] = parent.prototype.constructor.effects[effectName];
    }

    return extendingCarousel;
};


/**
 * merge 2 array/value of config
 */
Carousel.merge = function(obj1, obj2) {
    // return obj1 if obj2 is undefined
    if( obj2 === undefined )
        return obj1

    // priority to obj2 if obj1 is not an object (so it's a leaf) or obj2 is an HTML Element
    // check if obj1 == null because in this case, its type is object.
    if( typeof(obj1) != 'object' || obj1 == null || obj2.tagName !== undefined )
        return obj2;
    
    // replace existant element of obj1 by same in obj2
    // and complete obj1 by obj2
    for (var attrname in obj2) {
        obj1[attrname] = Carousel.merge(obj1[attrname], obj2[attrname]);
    }

    return obj1;
}

/**
 * Clone correctly an object (real clone not a copy by reference)
 */
Carousel.clone = function(obj)
{
    if(typeof(obj) != 'object' || obj == null)
        return obj;
    
    // to obtain an object of the same class
    var newObj = new obj.constructor();
    // copy each element into the new instance
    for(var i in obj)
        newObj[i] = Carousel.clone(obj[i]);
    
    return newObj;
}

Carousel.effects = {};

/**
 * Add comportement to facilitate the creation of a carousel
 */
{
    switch(Carousel.detectUsedLibrary())
    {
        case 'jquery':
            $.prototype.extend({setCarousel: function(config, parent){
                return Carousel.create(this[0], config, parent);
            }});
        break;
        
        
        case 'prototype':
            Element.addMethods({setCarousel: function(element, config, parent){
                return Carousel.create(element, config, parent);
            }});
        break;
        
        
        default:
            var isIE7 = !window.Element
            if(isIE7)
                Element = function(){};

            Element.prototype.setCarousel = function(config, parent) {
                return Carousel.create(this, config, parent);
            }

            if(isIE7)
            {
                var __createElement = document.createElement;
                document.createElement = function(tagName)
                {
                    var element = __createElement(tagName);
                    for(var key in Element.prototype)
                            element[key] = Element.prototype[key];
                    return element;
                }

                var __getElementById = document.getElementById
                document.getElementById = function(id)
                {
                    var element = __getElementById(id);
                    for(var key in Element.prototype)
                        element[key] = Element.prototype[key];
                    return element;
                }
            }
        break;
    }
}

Carousel.prototype = {
    attributes: {
        changeInProgress    : false,
        container           : null,
        current_shift       : 0,
        direction           : 'forward',
        effect              : null,
        elements            : null,
        elementsContainer   : null,
        elementsOrdered     : {},
        elementsCount       : 0,
        shortcuts           : {},
        thumbnails          : {},
        timer               : null,
        closestLinkAppend   : false,
        baseContainerClass  : 'carousel_elements'
    },

    config: {
        autostart           : true,
        autorotation        : true,
        widthElement        : null,
        heightElement       : null,
        displayedElement    : null,
        needMask            : false,
        navigation: {
            type            : 'shortcut',
            container       : null,
            label: {
                previous        : 'Previous',
                next            : 'Next' 
            },
            thumbnails          : {}
        }
    },

    events: {
        beforenext        : new Array(),
        beforeprevious    : new Array(),
        change            : new Array(),
        init              : new Array(),
        aftereffect       : new Array()
    },
    
    userConfig: {},

    constructor: Carousel,
    
    /**
     * constructor
     */
    construct: function(container, config)
    {
        if( config == undefined )
            config = {};
        
        // check the global container
        if (typeof(container) != 'object')
        {
            alert('Carousel - You must provide a target container');
            return false;
        }
        // on verifie egalement que c'est un element HTML
        else if (container.nodeName == undefined)
        {
            alert('Carousel - The target container must be an HTML element');
            return false;
        }
        else 
            this.attributes.container = container;
        
        
        // if there is no elements provided, check if the container as child and use them if possible
        if (typeof(config.elements) != 'object')
        {
            var elements = container.childNodes;
            config.elements = {};
            for(var i=0; i<elements.length; i++)
            {
                if(elements[i].nodeType == 1)
                    config.elements[i]=elements[i];
            }
        }
        
            
        // check there is at least one elements into the carousel
        if( config.elements.length == 0)
        {
            alert('Carousel - You must provide an array/object of elements or the container must directly contains the elements');
            return false;
        }
        
        // if the elements is into an array, tranform it into an object
        if(typeof(config.elements.length) != "undefined") // elements is in an array
        {
            this.attributes.elements = {};
            //for each array style
            for (var j = 0; j < config.elements.length; j++)
                this.attributes.elements[j+1] = config.elements[j];
        } else 
            this.attributes.elements = config.elements;

        // if elements to add is not provide into a 'tag' entry
        // create 'tag' entry
        for (var j in this.attributes.elements)
        {
            if( this.attributes.elements[j].tag == undefined )
                this.attributes.elements[j] = {tag: this.attributes.elements[j]};
        }

        // check all other configuration provided
        if (typeof(config) == 'object')
        {
            for (var attrname in config) {
                switch( attrname )
                {
                    case 'events':
                        for (var eventname in config[attrname]) {
                            if (this.events[eventname] !== undefined && typeof( config[attrname][eventname]) == 'function')
                                this.events[eventname].push(config[attrname][eventname]);
                        }
                    break;
                    
                    case 'effect':
                        this.loadEffect(config[attrname]);
                    break;
                    
                    default:
                        if( this.config[attrname] !== undefined  )
                            this.config[attrname] = Carousel.merge(this.config[attrname], config[attrname]);
                        else
                            this.userConfig[attrname] = config[attrname];
                    break;
                }               
           }
       }
       // load Base effect by default       
       if( this.attributes.effect == null)
            this.loadEffect();

       // if no widthElement provided, calculate it
       if (this.config.widthElement == null)
           this.config.widthElement = this.attributes.elements[1].tag.offsetWidth;

       // if no heightElement provided, calculate it
       if (this.config.heightElement == null)
           this.config.heightElement = this.attributes.elements[1].tag.offsetHeight;
       
       // if no displayedElement provided, calculate it
       if (this.config.displayedElement == null)
       {
           switch(this.attributes.effect.direction)
           {
               case 'top':
               case 'bottom':
                   this.config.displayedElement = Math.floor(this.attributes.container.offsetHeight / this.config.heightElement);
               break;
               
               case 'left':
               case 'right':
               default:
                   this.config.displayedElement = Math.floor(this.attributes.container.offsetWidth / this.config.widthElement);
               break;
           }
       }  

       // initialize the carousel
       this._init();

       // start the rotation if needed
       if (this.config.autostart) 
       {
           // force automatic rotation (autostart without autorotation is a nonsense)
           this.config.autorotation = false; // to false because the play function switch to true after checking false
           this.play();
       }
    },

    /**
     * display the carousel
     */ 
    _init: function()
    {        
        if( this.config.needMask )
        {
            // mask creation
            var mask = document.createElement('div');
            mask.className = 'mask';
            // add the mask to the global container
            this.attributes.container.appendChild(mask);
        }
        
        
        // create the carousel element container
        this.attributes.elementsContainer = document.createElement('div');
        this.attributes.elementsContainer.className = this.attributes.baseContainerClass;
        // add the carousel element container to the global container 
        this.attributes.container.appendChild(this.attributes.elementsContainer);

        if( this.config.navigation.type !== '' )
        {
            // create the navigation container if necessarily
            if (this.config.navigation.container == null)
            {
                this.config.navigation.container = document.createElement('div');
                this.config.navigation.container.className = this.config.navigation.type.replace('|', ' ');
            }

            // add the navigation container to the global container if it has not already a parent
            if (this.config.navigation.container.parentNode == undefined)
                this.attributes.container.appendChild(this.config.navigation.container);
        }

        // add carousel elements to the carousele element container
        for (var j in this.attributes.elements)
            this._insertElementIntoContainer(j);

        // _initialization of the elements array
        this._changeElementsOrder();
        
        this._setElementsContainerClassName();

        // trigger the _init event
        var id = this.attributes.elementsOrdered[0].getAttribute('id_carousel_element');
        this._triggerEvent('init', this.attributes.elements[id], this);

        return this;
    },

    /**
     * add an element to the global container and its shortcut to the shortcut container
     */
    _insertElementIntoContainer: function(id_element) {
        // Create the element
        var element = document.createElement('div');
        // if the content element is transmitted in text mode
        if( typeof(this.attributes.elements[id_element].tag) != 'object' )
            element.innerHTML = this.attributes.elements[id_element].tag; // defined the innerHTML
        else
            element.appendChild(this.attributes.elements[id_element].tag); // add the element content

        element.childNodes[0].setAttribute('id_carousel_element', id_element);
        this.attributes.elementsContainer.appendChild(element.childNodes[0]);

        // stocke le nombre d'element du carousel
        this.attributes.elementsCount++;

        if( this.config.navigation.type.search('shortcut') != -1 )
        {
            // create the shortcut
            var shortcut = document.createElement('a');
            shortcut.href = '#';
            shortcut.innerHTML = id_element;
            shortcut.setAttribute('id_carousel_element', id_element);
            shortcut.carousel = this;
            shortcut.onclick = function(){
                this.carousel.goToElement(this.getAttribute('id_carousel_element'));
                return false;
            };
            // add the shortcut to the shortcut container
            this.config.navigation.container.appendChild(shortcut);

            this.attributes.shortcuts[id_element] = shortcut;
        }

        if( this.config.navigation.type.search('thumbnail') != -1 )
        {
            // create the thumbnails
            var thumbnail = document.createElement('a');
            thumbnail.href = '#';
            if( typeof(this.config.navigation.thumbnails[id_element-1]) == 'object' )
                thumbnail.appendChild(this.config.navigation.thumbnails[id_element-1]);
            else if( this.config.navigation.thumbnails[id_element-1] != undefined )
                thumbnail.innerHTML = this.config.navigation.thumbnails[id_element-1];
            thumbnail.setAttribute('id_carousel_element', id_element);
            thumbnail.carousel = this;
            thumbnail.onclick = function(){
                this.carousel.goToElement(this.getAttribute('id_carousel_element'));
                return false;
            };
            // add the shortcut to the shortcut container
            this.config.navigation.container.appendChild(thumbnail);

            this.attributes.thumbnails[id_element] = thumbnail;
        }
        
        // Create the previous and next link if there is more element than visible in the global container
        if( this.config.navigation.type.search('closest') != -1 && (this.attributes.elementsCount > this.config.displayedElement) && !this.attributes.closestLinkAppend )
        {
            // Previous
            var previous = document.createElement('a');
            previous.href = '#';
            previous.className = 'previous';
            previous.innerHTML = this.config.navigation.label.previous;
            previous.carousel = this;
            previous.onclick = function(){
                this.carousel.previousElement();
                return false;
            };
            // add the link to the navigation container
            this.config.navigation.container.appendChild(previous);

            // Next
            var next = document.createElement('a');
            next.href = '#';
            next.className = 'next';
            next.innerHTML = this.config.navigation.label.next;
            next.carousel = this;
            next.onclick = function(){
                this.carousel.nextElement();
                return false;
            };
            // add the link to the navigation container
            this.config.navigation.container.appendChild(next);

            // indicate that this links already append
            this.attributes.closestLinkAppend = true;
        }
    },

    /**
     * load a timer for the next movement of the elements
     */
    _loadTimer: function(time) {
        var $this = this;
        if (time !== null && this.attributes.elementsCount > this.config.displayedElement)
        {
            if (time > 0)
                this.attributes.timer = window.setTimeout(function(){$this.change()}, time);
            else
                this.change();
        }
        return this;
    },

    /**
     * change ordered elements order and update shortcut if needed
     */
    _changeElementsOrder: function() {
        var elements = this.attributes.elementsContainer.childNodes,
            current_id = 0,
            el = null;
        this.attributes.elementsOrdered = {};

        if( elements.length > 0 )
        {    
            for (var i=0; i<elements.length; i++)
                this.attributes.elementsOrdered[i] = elements[i];

            // store current visible element ID
            current_id = this.attributes.elementsOrdered[0].getAttribute('id_carousel_element');

            // switch 'current' class on shortcut
            if( this.config.navigation.type.search('shortcut') != -1 )
            {
                // browse elements to add a className
                for (id_carousel_element in this.attributes.shortcuts)
                {
                    el = this.attributes.shortcuts[id_carousel_element];
                    el.className = el.className.replace('current', '');
                    if(id_carousel_element == current_id)
                        el.className = el.className + ' current';
                }
            }
            
            // switch 'current' class on thumbnails
            if( this.config.navigation.type.search('thumbnails') != -1 )
            {
                // browse elements to add a className
                for (id_carousel_element in this.attributes.thumbnails)
                {
                    el = this.attributes.thumbnails[id_carousel_element];
                    el.className = el.className.replace('current', '');
                    if(id_carousel_element == current_id)
                        el.className = el.className + ' current';
                }
            }

            // trigger change event
            this._triggerEvent('change', this.attributes.elements[current_id]);
        }

        return this;
    },

    /**
     * go directly to an element by its ID, without effect
     */
    _changeActiveElement: function(id_carousel_element, needTimeout) {
        var found = false;

        if( needTimeout == undefined )
            needTimeout = true;

        // stop timer
        window.clearTimeout(this.attributes.timer);

        // re_init position
        this.attributes.effect.resetPosition();

        // browse element to find the good by its ID
        for (i in this.attributes.elementsOrdered)
        {
            if (!found && this.attributes.elementsOrdered[i].getAttribute('id_carousel_element') != id_carousel_element)
                this.attributes.elementsOrdered[i].parentNode.appendChild(this.attributes.elementsOrdered[i]);
            else if (this.attributes.elementsOrdered[i].getAttribute('id_carousel_element') == id_carousel_element)
                found = true;

            // re_init element display style
            this.attributes.elementsOrdered[i].style.display = '';
        }
        this._changeElementsOrder();

        // relaunch timer if needed
        if (this.config.autorotation && needTimeout) this._loadTimer(this.attributes.effect.config.pause);

        return this;
    },

    /**
     * Launch the function correspond to an event name
     * @param eventname - name of the event
     * @param params - additional parameters passed to the event function
     */
    _triggerEvent: function() {
        var params = Array.prototype.slice.call(arguments);//, 0, arguments.length);
        var eventname = params.shift();
        if (this.events[eventname] != undefined)
        {
            for( var i=0; i<this.events[eventname].length; i++ )
                this.events[eventname][i].apply(this, params);
        }

        return this;
    },

    /**
     * check if the selected transition is usable
     */
    _checkEffectValidity: function() {
        return (this.attributes.effect !== null);
    },
    
    /**
     *   add/change the classname of the element container (associated to the effect)
     */
    _setElementsContainerClassName: function(){
        if( this.attributes.elementsContainer !== null)
            this.attributes.elementsContainer.className = this.attributes.baseContainerClass + ' ' + this.attributes.effect.getClass();
    },
    
    /**
     * Change effect
     */
    loadEffect: function(config)
    {
        if( typeof(config) != 'object' )
        {
            // default effect
            config = {
                type: 'base',
                params: {}
            };
        }
        
        var found = false;
        // Effects management
        for(var effectName in this.constructor.effects)
        {
            if( config.type.toLowerCase() == effectName.toString().toLowerCase() )
            {
                var effect = new this.constructor.effects[effectName]();
                effect.link(this, config.params);
                
                if( this.attributes.effect != null && this.attributes.effect.inProgress )
                {
                    $carousel = this;
                    this.events.aftereffect.push(function(){
                        $carousel.attributes.effect = effect;
                        $carousel.events.aftereffect.pop();
                    })
                }
                else
                    this.attributes.effect = effect;
                
                this._setElementsContainerClassName();
                found = true;
            }
        }
        
        if( !found )
           alert('Carousel - this.attributes.effect (' + config.type + ') is not available');
        
        return this;
    },
    
    /**
     * switch between the different type of movement to change element
     */
    change: function() {
        // clear timeout to be sure there is no conflict
        window.clearTimeout(this.attributes.timer);
        this._setElementsContainerClassName();
        this.attributes.effect.update();
    },
    
    /**
     * go to the next element without effect
     */
    goToNext: function(needTimeout) {
        var id;
        if( this.attributes.direction == 'forward')
            id = this.attributes.elementsOrdered[1].getAttribute('id_carousel_element');
        else
            id = this.attributes.elementsOrdered[this.attributes.elementsCount-1].getAttribute('id_carousel_element');
        
        return this._changeActiveElement(id, needTimeout);
    },

    /**
     * go to an element, identified by its ID, using effect
     */
    goToElement: function(id_carousel_element) {
        var $carousel = this,
            original_ordered = this.attributes.elementsOrdered,
            current_id_element = original_ordered[0].getAttribute('id_carousel_element'),
            new_ordered = new Array(),
            elements = this.attributes.elementsContainer.childNodes,
            cursor = 0;
        // change only if no effec in progress
        if( current_id_element != id_carousel_element && !this.attributes.effect.inProgress )
        {
            // reinit the element ordered
            this.attributes.elementsOrdered = {};

            // create the temporary order
            for (var i=0; i<elements.length; i++)
            {
                // if the element is the wanted element ...
                if( elements[i].getAttribute('id_carousel_element') == id_carousel_element)
                {
                    // place it into the good box
                    if( this.attributes.direction == 'forward')
                        new_ordered[1] = elements[i];
                    else
                        new_ordered[elements.length] = elements[i];
                }
                else
                {
                    // place other elements into the other boxes
                    if( this.attributes.direction == 'forward' & cursor == 1)
                        cursor++;

                    new_ordered[cursor++] = elements[i];
                }
            }
            
            // reorder "physically" the elements regards to the new order 
            for(var i=0; i<new_ordered.length; i++)
            {
                this.attributes.elementsOrdered[i] = new_ordered[i];
                this.attributes.elementsContainer.appendChild(new_ordered[i]);
            }

            // reinit the original order after the effect
            this.events.aftereffect.push(function(){
                $carousel.events.aftereffect.pop();
                $carousel.attributes.elementsOrdered = original_ordered;
                for(var i in original_ordered)
                    this.attributes.elementsContainer.appendChild(original_ordered[i]);
                $carousel._changeActiveElement(id_carousel_element);
            });

            // launch the change by effect
            this.change();
        }
        
        return this;
    },
    
    changeDirection: function(direction) {
        this.attributes.direction = direction;
        return this;
    },

    /**
     * go to the next element
     */
    nextElement: function() {
        // trigger beforenext event
        this._triggerEvent('beforenext');
        if (this.attributes.elementsCount > this.config.displayedElement && !this.attributes.effect.inProgress)
        {
            this.attributes.effect.prepareNextElement();
            this.change();
        }

        return this;
    },

    /**
     * go to the previous element
     */
    previousElement: function() {
        // trigger beforeprevious event
        this._triggerEvent('beforenprevious');
        if (this.attributes.elementsCount > this.config.displayedElement && !this.attributes.effect.inProgress)
        {
            this.attributes.effect.preparePreviousElement();
            this.change();
        }

        return this;
    },

    /**
     * stop the rotation
     */
    stop: function() {
        this.config.autorotation = false;
        
        if( !this.attributes.effect.inProgress )
            window.clearTimeout(this.attributes.timer);
        
        return this;
    },

    /**
     * start the rotation
     */
    play: function() {
        if( !this.config.autorotation )
        {
            this.config.autorotation = true;
            window.clearTimeout(this.attributes.timer);
            this._loadTimer(this.attributes.effect.config.pause);  
        }

        return this;
    }
};



/**
 ********************************************************************************
 * EFFECTS
 */

/*******************
 * Effect.Base
 * 
 * params:
 *      pause   : time during which an element is static
 */
Carousel.effects.Base = function(){};

Carousel.effects.Base.extend = function(config, attributes, parent){
    if( parent == undefined )
        parent = Carousel.effects.Base;
    
    if( config == undefined )
        config = {};
    
    if( attributes == undefined )
        attributes = {};
    
    var prototype = new parent.prototype.constructor();
    if( (prototype instanceof Carousel.effects.Base) == false )
    {
        parent = Carousel.effects.Base;
        prototype = new parent.prototype.constructor();
    }

    var extendingEffect = function(){
        parent.prototype.constructor.call(this, arguments);
        // to avoid each instance has the same reference to "object"
        for(attrname in this)
        {
            if( typeof(this[attrname]) == 'object' )
                this[attrname] = Carousel.clone(this[attrname]);
        }
    };
    extendingEffect.prototype = Carousel.clone(prototype);
    extendingEffect.prototype.constructor = extendingEffect;
    
    Carousel.merge(extendingEffect.prototype.config, config);
    Carousel.merge(extendingEffect.prototype.attributes, attributes);

    return extendingEffect;
};

Carousel.effects.Base.prototype = {
    carousel: null,
    inProgress: false,
    config: {
        pause           : 4000
    },
    
    attributes: {
        name: 'Base'
    },
    
    constructor: Carousel.effects.Base,
    
    link: function(carousel, config) {
        this.carousel = carousel;
        Carousel.merge(this.config, config);
    },
    
    update: function() {
        var $carousel = this.carousel;
        
        $carousel.goToNext();
        
        var id = $carousel.attributes.elementsOrdered[0].getAttribute('id_carousel_element');
        $carousel._triggerEvent('aftereffect', $carousel.attributes.elements[id]);
    },
    
    move: function() {},
    
    resetPosition: function() {},
    
    preparePreviousElement: function() {
        var $carousel = this.carousel;
        
        $carousel.changeDirection('backward');
        $carousel.events.change.push(function(){
            $carousel.changeDirection('forward');
            $carousel.events.change.pop();
        });
    },
    
    prepareNextElement: function() {},
    
    getClass: function() {
        return 'effect-' + this.attributes.name.toLowerCase()
    }
}


/*******************
 * Effect.Slide
 * 
 * params herited from the Effect.Base
 * specific params:
 *      direction   : the direction of the sliding
 *      duration    : time during which the element is in movement
 */

Carousel.effects.Slide = Carousel.effects.Base.extend({direction: 'left', duration: 500}, {name:'slide', current_shift: 0, tempo: 20});

Carousel.effects.Slide.prototype.update = function() {
    var time = this.attributes.tempo,
        $carousel = this.carousel;
    
    if( !this.inProgress )
    {
        // if the movement is to the right - call the last element to the first place outer of the carousel "window"
        if( (this.config.direction == 'right' || this.config.direction == 'bottom') )
        {
            $carousel.changeDirection('backward');
            // call the last element to the first place
            $carousel.goToNext(false);
            // shift this one to the left by its width
            this.move();
        }
        else
            $carousel.changeDirection('forward');
    }
    
    this.inProgress = true;

    // define the new position
    this.attributes.current_shift = this._getUnitShift();

    // slide elements
    this.move();
    
    if( this.attributes.current_shift < 0 || this.attributes.current_shift > this._getMaxDimension() )
    {
        if (this.config.direction == 'left' || this.config.direction == 'top')
            $carousel.goToNext(false);
        else
            this.resetPosition();
        
        this.inProgress = false;
        
        // trigger the after effect event
        var id = $carousel.attributes.elementsOrdered[0].getAttribute('id_carousel_element');
        $carousel._triggerEvent('aftereffect', $carousel.attributes.elements[id]);
        
        time = ( $carousel.config.autorotation) ? this.config.pause : null;
    }

    // relaunch the timer
    $carousel._loadTimer(time);
}

Carousel.effects.Slide.prototype.move = function() {
    var $carousel = this.carousel,
        elementStyle = $carousel.attributes.elementsOrdered[0].style,
        value = this.attributes.current_shift; 
    switch(this.config.direction)
    {
        case 'left':
            elementStyle.marginLeft = value*-1 + 'px';
        break;
        case 'right':
            elementStyle.marginLeft = (this._getMaxDimension() - value)*-1 + 'px';
        break;            
        case 'top':
            elementStyle.marginTop = value*-1 + 'px';
        break;
        case 'bottom':
            elementStyle.marginTop = (this._getMaxDimension() - value)*-1 + 'px';
        break;
    }
    return this;     
}

/**
 * when the previous element is called:
 *  - invert the direction 
 *  - add event to restore direction after the effect
 */
Carousel.effects.Slide.prototype.preparePreviousElement = function() {
    var $carousel = this.carousel;
    
    this.invertDirection();
    $carousel.events.aftereffect.push(function(){
        $carousel.attributes.effect.invertDirection();
        $carousel.events.aftereffect.pop();
    });
}

/**
 * return the unit shift during the sliding effect
 */ 
Carousel.effects.Slide.prototype._getUnitShift = function() {
    var nbInterval = this.config.duration/this.attributes.tempo,
        step = this._getMaxDimension() / nbInterval;
        
    return (parseInt(this.attributes.current_shift) + step);
}
    
/**
 * return the max dimension relative to the direction
 */
Carousel.effects.Slide.prototype._getMaxDimension = function() {
    var $carousel = this.carousel;
    
    switch(this.config.direction)
    {
        case 'left':
        case 'right':
            return $carousel.config.widthElement;
        break;

        case 'top':
        case 'bottom':
            return $carousel.config.heightElement;
        break;
    }
}

/**
 * Change the direction to the opposite
 */
Carousel.effects.Slide.prototype.invertDirection = function() {
    var direction;
    // invert direction
    switch(this.config.direction)
    {
         case 'top':
             direction = 'bottom';
         break;
         case 'right':
             direction = 'left';
         break;
         case 'bottom':
             direction = 'top';
         break;
         case 'left':
             direction = 'right';
         break;
    }

    this.config.direction = direction;
    return this;
}
    
/**
 * Reset the margin of each element and the curent_shift to 0
 */
Carousel.effects.Slide.prototype.resetPosition = function() {
    var $carousel = this.carousel;
    
    for (i in $carousel.attributes.elementsOrdered)
    {
        var element = $carousel.attributes.elementsOrdered[i].style;
        element.marginTop = element.marginRight = element.marginBottom = element.marginLeft = '0px';
    }
    this.attributes.current_shift = 0;
}

/**
 * Add the direction to the class
 */
Carousel.effects.Slide.prototype.getClass = function() {
    var direction = 'horizontal',
        classname = Carousel.effects.Base.prototype.getClass.apply(this, arguments);
    switch(this.config.direction)
    {
         case 'top':
         case 'bottom':
             direction = 'vertical';
         break;
    }
    
    return classname + ' effect-' + direction + '-move'
}

/*******************
 * Effect.Swing
 * 
 * params herited from the Effect.Slide
 */
Carousel.effects.Swing = Carousel.effects.Base.extend({}, {name:'swing', firstPart: true}, Carousel.effects.Slide);

Carousel.effects.Swing.prototype.link = function(){
    Carousel.effects.Slide.prototype.link.apply(this, arguments);
    
    this.attributes.tempo = this.attributes.tempo / 2;
};

Carousel.effects.Swing.prototype.update = function() {
    var $carousel = this.carousel,
        time = this.attributes.tempo;

    this.inProgress = true;
    
    // define the new position
    this.attributes.current_shift = this._getUnitShift();

    this.move();

    if( this.attributes.current_shift < 0 || this.attributes.current_shift > this._getMaxDimension() )
    {            
        // End of the first part
        if( this.attributes.firstPart )
        {
            $carousel.goToNext(false);
            
            this.attributes.current_shift = this._getMaxDimension();
            this.move();
            this.attributes.current_shift = 0;
        }
        // End of the second part
        else
        {
            this.resetPosition();
            this.inProgress = false;
            
            // trigger the after effect event
            var id = $carousel.attributes.elementsOrdered[0].getAttribute('id_carousel_element');
            $carousel._triggerEvent('aftereffect', $carousel.attributes.elements[id]);
            
            // relaunch timer for the next iteration
            time = ( $carousel.config.autorotation) ? this.config.pause : null;
        }
            
        this.invertDirection();

        this.attributes.firstPart = !this.attributes.firstPart;
    }

    $carousel._loadTimer(time)
}

Carousel.effects.Swing.prototype.move = function() {
    var $carousel = this.carousel,
        elementStyle = $carousel.attributes.elementsOrdered[0].style,
        value = this.attributes.current_shift;

    switch(this.config.direction)
    {
        case 'left':
            if( this.attributes.firstPart )
            {
                elementStyle.marginLeft = value*-1 + 'px';
                elementStyle.marginRight = value + 'px';
            }
            else
                elementStyle.marginLeft = (this._getMaxDimension() - value) + 'px';
        break;
        case 'right':
            if( !this.attributes.firstPart )
            {
                value = (this._getMaxDimension() - value);
                elementStyle.marginLeft = value*-1 + 'px';
                elementStyle.marginRight = value + 'px';
            }
            else
                elementStyle.marginLeft = value + 'px';
        break;            
        case 'top':
            if( this.attributes.firstPart )
            {
                elementStyle.marginTop = value*-1 + 'px';
                elementStyle.marginBottom = value + 'px';
            }
            else
                elementStyle.marginBottom = value + 'px';
        break;
        case 'bottom':
            if( !this.attributes.firstPart )
            {
                value = (this._getMaxDimension() - value);
                elementStyle.marginTop = value*-1 + 'px';
                elementStyle.marginBottom = value + 'px';
            }
            else
                elementStyle.marginBottom = value + 'px';
        break;
    }
    return this;
}

Carousel.effects.Swing.prototype.preparePreviousElement = function() {
    Carousel.effects.Base.prototype.preparePreviousElement.apply(this, arguments);
}


/*******************
 * Effect.SlideHover
 * 
 * params herited from the Effect.Slide
 */
Carousel.effects.SlideHover = Carousel.effects.Base.extend({}, {name:'slide-hover'}, Carousel.effects.Slide);

Carousel.effects.SlideHover.prototype.move = function() {
    var $carousel = this.carousel,
        elementStyle = $carousel.attributes.elementsOrdered[0].style,
        nextStyle = $carousel.attributes.elementsOrdered[1].style,
        value = this.attributes.current_shift; 
        
    switch(this.config.direction)
    {
        case 'left':
            elementStyle.position = nextStyle.position = '';
            elementStyle.marginRight = value*-1 + 'px';
        break;
        case 'right':
            elementStyle.marginLeft = (this._getMaxDimension() - value)*-1 + 'px';
            elementStyle.marginRight = value*-1 + 'px';
            elementStyle.position = 'relative'
            nextStyle.position = 'relative';
            elementStyle.zIndex = 1;
            nextStyle.zIndex = 0;
        break;  
        case 'top':
            elementStyle.position = nextStyle.position = '';
            elementStyle.marginBottom = value*-1 + 'px';
        break;
        case 'bottom':
            elementStyle.marginTop = (this._getMaxDimension() - value)*-1 + 'px';
            elementStyle.marginBottom = value*-1 + 'px';
            elementStyle.position = 'relative'
            nextStyle.position = 'relative';
            elementStyle.zIndex = 1;
            nextStyle.zIndex = 0;
        break;  
    }
    return this;     
}

/**
 ********************************************************************************
 * JQuery Carousel Version
 */
var JQueryCarousel = Carousel.extend();

/*******************
 * Effect.Fade (for JQuery Library)
 * 
 * params herited from the Effect.Base
 * specific params:
 *      duration    : time during which the element is in movement
 */
JQueryCarousel.effects.Fade = Carousel.effects.Base.extend({duration: 500}, {name:'fade'});

JQueryCarousel.effects.Fade.prototype.link = function(){
    Carousel.effects.Base.prototype.link.apply(this, arguments);
    this.config.duration = this.config.duration/2;
};

JQueryCarousel.effects.Fade.prototype.update = function() {
    var $carousel = this.carousel,
        $this = this;
    this.inProgress = true;
    $($carousel.attributes.elementsOrdered[0]).fadeOut(this.config.duration, function(){$this._endUpdate()});
    
    return this;
}

JQueryCarousel.effects.Fade.prototype._endUpdate = function() {
    var $carousel = this.carousel,
        $this = this;
    
    $carousel.goToNext();
    
    $carousel.attributes.elementsOrdered[0].style.display = 'none';
    $($carousel.attributes.elementsOrdered[0]).fadeIn(  this.config.duration, 
                                                        function(){
                                                            $this.inProgress = false;
                                                            // trigger the after effect event
                                                            var id = $carousel.attributes.elementsOrdered[0].getAttribute('id_carousel_element');
                                                            $carousel._triggerEvent('aftereffect', $carousel.attributes.elements[id]);
                                                        }
                                                    );
    
    return this;
}


/*******************
 * Effect.FadeHover (for JQuery Library)
 * 
 * params herited from the Effect.Base
 * specific params:
 *      duration    : time during which the element is in movement
 */
JQueryCarousel.effects.FadeHover = Carousel.effects.Base.extend({duration: 500}, {name:'fade-hover'});

JQueryCarousel.effects.FadeHover.prototype.update = function() {
    var $carousel = this.carousel,
        $this = this,
        firstElement = $carousel.attributes.elementsOrdered[0],
        secondElement = null,
        time = ( $carousel.config.autorotation) ? this.config.pause : null;;
    this.inProgress = true;
    
    firstElement.style.position = 'absolute';
    firstElement.style.left = '0px';
    firstElement.style.top = '0px';
    $(firstElement).fadeOut(this.config.duration);
    
    $carousel.goToNext(false);
    secondElement = $carousel.attributes.elementsOrdered[0];
    secondElement.style.display = 'none';
    $(secondElement).fadeIn(    this.config.duration, 
                                function(){
                                    firstElement.style.position = '';
                                    $carousel._loadTimer(time);
                                    $this.inProgress = false;
                                    // trigger the after effect event
                                    var id = firstElement.getAttribute('id_carousel_element');
                                    $carousel._triggerEvent('aftereffect', $carousel.attributes.elements[id]);
                                } 
                            );
    
    return this;
}


/**
 ********************************************************************************
 * Prototype Carousel Version
 */
var PrototypeCarousel = Carousel.extend();

/*******************
 * Effect.Fade (for Prototype Library)
 * 
 * params herited from the Effect.Base
 * specific params:
 *      duration    : time during which the element is in movement
 */
PrototypeCarousel.effects.Fade = Carousel.effects.Base.extend({duration: 500}, {name:'fade'});

PrototypeCarousel.effects.Fade.prototype.link = function(){
    Carousel.effects.Base.prototype.link.apply(this, arguments);
    this.config.duration = this.config.duration/2000;
};

PrototypeCarousel.effects.Fade.prototype.update = function() {
    var $carousel = this.carousel,
        $this = this;
    this.inProgress = true;

    var options = {
        duration: this.config.duration, 
        from: 1.0, 
        to: 0.0,
        afterFinish: function(){$this._endUpdate()}
    };
    new Effect.Opacity($carousel.attributes.elementsOrdered[0], options);

    return this;
}

PrototypeCarousel.effects.Fade.prototype._endUpdate = function() {
    var $carousel = this.carousel,
        $this = this;
    
    $carousel.goToNext();
    
    var options = {
        duration: this.config.duration, 
        from: 0.0, 
        to: 1.0,
        afterFinish: function(){
            $this.inProgress = false;
            // trigger the after effect event
            var id = $carousel.attributes.elementsOrdered[0].getAttribute('id_carousel_element');
            $carousel._triggerEvent('aftereffect', $carousel.attributes.elements[id]);
        }
    };
    new Effect.Opacity($carousel.attributes.elementsOrdered[0], options);
            
    return this;
}


/*******************
 * Effect.FadeHover (for prototype Library)
 * 
 * params herited from the Effect.Base
 * specific params:
 *      duration    : time during which the element is in movement
 */
PrototypeCarousel.effects.FadeHover = Carousel.effects.Base.extend({duration: 500}, {name:'fade-hover'});

PrototypeCarousel.effects.FadeHover.prototype.link = function(){
    Carousel.effects.Base.prototype.link.apply(this, arguments);
    this.config.duration = this.config.duration/1000;
};

PrototypeCarousel.effects.FadeHover.prototype.update = function() {
    var $carousel = this.carousel,
        $this = this,
        firstElement = $carousel.attributes.elementsOrdered[0],
        secondElement = null,
        optionFirst = {
            duration: this.config.duration, 
            from: 1.0, 
            to: 0.0
        },
        optionSecond = {
            duration: this.config.duration, 
            from: 0.0, 
            to: 1.0
        }
        time = ( $carousel.config.autorotation) ? this.config.pause : null;
    this.inProgress = true;
    
    firstElement.style.position = 'absolute';
    firstElement.style.left = '0px';
    firstElement.style.top = '0px';
    new Effect.Opacity(firstElement, optionFirst);
    
    $carousel.goToNext(false);
    secondElement = $carousel.attributes.elementsOrdered[0];
    optionSecond.afterFinish = function(){
                                    firstElement.style.position = '';
                                    $carousel._loadTimer(time);
                                    $this.inProgress = false;
                                    // trigger the after effect event
                                    var id = firstElement.getAttribute('id_carousel_element');
                                    $carousel._triggerEvent('aftereffect', $carousel.attributes.elements[id]);
                                };
    new Effect.Opacity(secondElement, optionSecond); 
    
    return this;
}