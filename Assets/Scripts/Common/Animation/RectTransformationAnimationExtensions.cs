// Copyright (c) 2015 Jonathan Parham

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the “Software”), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using UnityEngine;

public static class RectTransformationAnimationExtensions
{
    public static Tweener AnchorTo(this RectTransform t, Vector3 position)
    {
        return AnchorTo(t, position, Tweener.DefaultDuration);
    }

    public static Tweener AnchorTo(this RectTransform t, Vector3 position, float duration)
    {
        return AnchorTo(t, position, duration, Tweener.DefaultEquation);
    }

    public static Tweener AnchorTo(this RectTransform t, Vector3 position, float duration, Func<float, float, float, float> equation)
    {
        RectTransformAnchorPositionTweener tweener = t.gameObject.AddComponent<RectTransformAnchorPositionTweener>();
        tweener.startTweenValue = t.anchoredPosition;
        tweener.endTweenValue = position;
        tweener.duration = duration;
        tweener.equation = equation;
        tweener.Play ();
        return tweener;
    }
}
