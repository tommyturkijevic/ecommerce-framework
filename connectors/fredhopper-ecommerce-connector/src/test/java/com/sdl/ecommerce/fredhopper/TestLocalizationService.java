package com.sdl.ecommerce.fredhopper;

import com.sdl.ecommerce.api.LocalizationService;
import org.springframework.beans.factory.annotation.Value;

import java.net.URI;
import java.util.HashMap;
import java.util.Map;

/**
 * TestLocalizationService
 *
 * @author nic
 */
public class TestLocalizationService implements LocalizationService {

    @Value("${fredhopper.universe")
    private String universe;

    @Value("${fredhopper.locale}")
    private String locale;

    @Override
    public String getLocale() {
        return locale;
    }

    @Override
    public String getLocalizedConfigProperty(String name) {
        if ( name.equals("fredhopper-universe") ) {
            return universe;
        }
        else if ( name.equals("fredhopper-locale" ) ) {
            return locale;
        }
        else if ( name.equals("fredhopper-productModelMappings") ) {
            return "name=name;description=description;price=price;thumbnailUrl=_thumburl;primaryImageUrl=_imageurl;variantId=style_number";
        }
        else if ( name.equals("fredhopper-triggerMappings") ) {
            return "taf:claim:segment=fh_keyword";
        }
        else {
            return null;
        }
    }


    @Override
    public Map<URI, Object> getAllClaims() {
        Map<URI,Object> claims = new HashMap<>();
        claims.put(URI.create("taf:claim:segment"), "TestSegment");
        return claims;
    }
}
