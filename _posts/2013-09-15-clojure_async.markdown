---
layout: post
title:  "Clojure core async"
date:   2013-09-15 04:15:52
categories: clojure
---

**Clojure.core.async** library introduces _'go like'_ channels.

This is a classic sample,
using goroutines to sum a list of integers in paralell ( 10 lightweight threads ), and then suming all the results.

* create a channel
{% highlight clojure %}
  (let [c (chan)])
{% endhighlight %}

* create a goroutine
{% highlight clojure %}
  (go ( ... ))
{% endhighlight %}

* push a value to the channel, inside a goroutine
{% highlight clojure %}
  (go (>! c (do-sum part)))
{% endhighlight %}

* receive all values from the channel
{% highlight clojure %}
  (for [_ (range 10)] (<!! c))
{% endhighlight %}

* `<!!` is used to pull the values from the channel, like `<!`,
  but in the main thread

* Here is the complete code :
{% highlight clojure %}
(ns sample-async.core
  (:require [clojure.core.async :refer :all])
  (:gen-class))

(def numbers (range 100))

(defn- do-sum [numbers]
    (reduce + numbers))

(defn -main [& args]
  (let [parts (partition 10 numbers)
        c (chan)]
    (doseq [part parts]
      (go
       (>! c (do-sum part))))

    (prn
     (str "Total sum = "
        (do-sum (for [_ (range 10)] (<!! c)))))))

{% endhighlight %}

---------------------------------------

#### Some interesting links

- [Clojure async documentation] [1]

- [**blog.drewolson.org**, clojure async blog post ] [2]

- [LightTable] [3], really impressive, with an inline instant repl, console, 
and with the possibility of eval any function in the moment !

![LightTable in action]({{ site.url }}/assets/lighttable.png)

[1]: http://clojure.github.io/core.async/        "Clojure async documentation"
[2]: http://blog.drewolson.org/blog/2013/07/04/clojure-core-dot-async-and-go-a-code-comparison/ "Sample blog post, from drew olson"
[3]: http://www.lighttable.com "LightTable"