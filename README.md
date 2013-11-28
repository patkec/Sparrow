Sparrow
=======

Simple offer managament - a prototype.

Description
-----------

Application is separated into several sections. Each section can be accessed from the top navigation bar.

###Administration###

In this section all application events can be observed in real time. Events include the following:

* User operations (create/edit/delete)
* Customer operations (create/edit/delete)
* Product operations (create/edit/delete)
* Offer operations (create offer from draft/offer won/offer lost)

###Users###

Basic CRUD views are provided for user management. A user represents someone, who can log into the application and
create offers for customers.

Login functionality itself is not implemented, users must be selected from a list of users instead of system assigning
the current user to an offer.

####Properties####

* Name - mandatory.
* Role - optional.
* Email - optional.

###Customers###

Basic CRUD views are provided for customer management. A customer represents an imaginary customer that would get 
an offer about one or more products.

####Properties####

* Name - mandatory.
* Email - optional.
* Rating - optional.

###Product###

Basic CRUD views are provided for product management. A product represents an item with some physical properties which 
can be put on an offer.

####Properties####

* Title - mandatory.
* Description - optional.
* Price - mandatory.

###Offers###

Offers are actually split into two separate entities. The first one is an *OfferDraft* and the second one the *Offer*.

####Workflow####

User must first create a draft before creating an actual offer. Each draft can be edited at will. Products can be added
or removed from a draft. Once the user is satisfied with the draft, it can select "Send offer to customer" action. This
action will create an offer based on current draft and draft itself will be moved to archived drafts. The following actions
are available for drafts:

* Send offer to customer - marks the draft as archived and creates a new offer based on the draft.
* Delete - deletes the draft. Only available for unarchived drafts.

Note that archived drafts can still be modified and can still be used to create new offers. They just cannot be deleted.

Offer itself cannot be modified in any way. The following actions are available for the offer:

* Convert to order - marks the offer as won (accepted by the customer). An actual order should then be created, but
  this is not a part of this prototype.
* Make a new offer - marks the offer as lost and creates a new draft based on current offer.
* Terminate offer - marks the offer as lost. This option is available only if the offer has expired.

####Properties####

*OfferDraft*

* Title - mandatory.
* Archived - indicates if draft is archived.
* Owner - mandatory. Indicates a user who created the draft, but can be changed.
* Customer - mandatory. Indicates for which user the offer will be for, but can be changed.
* Discount - optional, defaults to 0. In percentage, applies to whole draft/offer.
* Items - a list of items on offer. Each item consists of a product, a quantity and a discount for the item.
* TotalPrice - calculated.
* SourceOffer - indicates a source offer, if draft was created from an offer.
* Offers - list of offers created from the draft.

*Offer*

* Title - title.
* Status - offered/won/lost.
* Owner - user who created the offer.
* Customer - customer for which the offer is for.
* Discount - discount in percentage.
* Items - a list of items on offer.
* TotalPrice - total price of the offer.
* Draft - indicates a source draft.